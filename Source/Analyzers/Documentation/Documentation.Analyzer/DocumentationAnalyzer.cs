using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DevTools.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace DevTools.Analyzers.Documentation
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DocumentationAnalyzer : DiagnosticAnalyzer
    {
        internal const string Category = "Documentation";

        private static readonly Regex CrefPattern = new Regex(@"<exception cref=""\S*"">");

        public static ExceptionNotDocumentedDiagnostic ExceptionNotDocumentedDiagnostic { get; } = new ExceptionNotDocumentedDiagnostic();
        public static ExceptionNotThrownDiagnostic ExceptionNotThrownDiagnostic { get; } = new ExceptionNotThrownDiagnostic();
        public static IncorrectExceptionDocumentationDiagnostic IncorrectExceptionDocumentationDiagnostic { get; } = new IncorrectExceptionDocumentationDiagnostic();
        public static InnerExceptionNotDocumentedDiagnostic InnerExceptionNotDocumentedDiagnostic { get; } = new InnerExceptionNotDocumentedDiagnostic();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.CreateRange(new[]
        {
            ExceptionNotThrownDiagnostic.Rule,
            IncorrectExceptionDocumentationDiagnostic.Rule,
            ExceptionNotDocumentedDiagnostic.Rule,
            InnerExceptionNotDocumentedDiagnostic.Rule
        });

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            SemanticModel semanticModel = context.SemanticModel;
            MethodDeclarationSyntax methodDeclaration = (MethodDeclarationSyntax)context.Node;

            if (methodDeclaration?.Body == null)
                return;
            
            List<ExceptionLocation> documentedExceptions = GetDocumentedExceptions(context, methodDeclaration);
            List<ExceptionLocation> thrownExceptions = GetThrownExceptions(semanticModel, methodDeclaration);

            foreach (ExceptionLocation exceptionLocation in documentedExceptions)
            {
                if(thrownExceptions.All(e => e.Exception != exceptionLocation.Exception))
                    context.ReportDiagnostic(ExceptionNotThrownDiagnostic.CreateDiagnostic(exceptionLocation.Location, exceptionLocation.Exception));
            }

            foreach (ExceptionLocation exceptionLocation in thrownExceptions)
            {
                if(documentedExceptions.All(e => e.Exception != exceptionLocation.Exception))
                    context.ReportDiagnostic(ExceptionNotDocumentedDiagnostic.CreateDiagnostic(exceptionLocation.Location, exceptionLocation.Exception));
            }
        }

        private static List<ExceptionLocation> GetThrownExceptions(SemanticModel semanticModel, MethodDeclarationSyntax methodDeclaration)
        {
            List<ExceptionLocation> thrownExceptions = new List<ExceptionLocation>();

            if (methodDeclaration?.Body?.Statements == null)
                return thrownExceptions;

            foreach (SyntaxNode syntaxNode in methodDeclaration.Body.Statements.SelectMany(s => s.DescendantNodesAndSelf()))
            {
                if (syntaxNode.Kind().IsAnyOf(SyntaxKind.ThrowStatement, SyntaxKind.ThrowExpression))
                {
                    thrownExceptions.AddRange(syntaxNode.GetExceptionSources()
                        .OfType<ObjectCreationExpressionSyntax>()
                        .Select(e => new ExceptionLocation(semanticModel.GetSymbolInfo(e).Symbol.ContainingType.ToString(), e.GetLocation())));
                }
                else
                {
                    if (semanticModel.GetSymbolInfo(syntaxNode).Symbol is ISymbol symbol)
                    {
                        string documentationString = symbol.GetDocumentationCommentXml();

                        if (string.IsNullOrEmpty(documentationString))
                            continue;

                        try
                        {
                            XDocument documentation = XDocument.Parse(documentationString);

                            List<string> exceptions = documentation.Descendants("exception")
                                .Select(e => e.Attribute("cref")?.Value.Substring(2))
                                .Where(e => !string.IsNullOrEmpty(e))
                                .Distinct()
                                .ToList();

                            if (exceptions.Any())
                            {
                                Location location = syntaxNode.GetLocation();

                                foreach (string exception in exceptions)
                                {
                                    ExceptionLocation exceptionLocation = new ExceptionLocation(exception, location);
                                    bool skipException = false;

                                    foreach (ExceptionLocation match in thrownExceptions.Where(exceptionLocation.OverlapsWith))
                                    {
                                        if (exceptionLocation.Span.Contains(match.Span))
                                            thrownExceptions[thrownExceptions.IndexOf(match)] = exceptionLocation;
                                        else if (match.Span.Contains(exceptionLocation.Span))
                                            skipException = true;
                                    }

                                    if (!skipException)
                                        thrownExceptions.Add(exceptionLocation);
                                }
                            }
                        }
                        catch
                        {
                            // Could not load symbol documentation.
                        }
                    }
                }
            }

            return thrownExceptions;
        }

        // TODO: Move exception messages to resources.
        private static List<ExceptionLocation> GetDocumentedExceptions(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
        {
            DocumentationCommentTriviaSyntax documentation = methodDeclaration.GetLeadingTrivia()
                .FirstOrDefault(t => t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia)
                .GetStructure() as DocumentationCommentTriviaSyntax;

            SemanticModel semanticModel = context.SemanticModel;

            List<ExceptionLocation> documentedExceptions = new List<ExceptionLocation>();

            if (documentation == null)
                return documentedExceptions;

            foreach (XmlElementSyntax xmlNode in documentation.Content.OfType<XmlElementSyntax>())
            {
                XmlElementStartTagSyntax startTag = xmlNode.StartTag;
                string tagName = startTag.Name.LocalName.Text.ToLower();

                switch (tagName)
                {
                    case "exception":
                    {
                        if (!CrefPattern.IsMatch(xmlNode.ToString()))
                        {
                            context.ReportDiagnostic(IncorrectExceptionDocumentationDiagnostic.CreateDiagnostic(startTag.GetLocation(), @"Xml tag should be of format '<exception cref=""ExceptionType"">'."));
                            continue;
                        }

                        XmlCrefAttributeSyntax crefAttribute = startTag.FindAttribute<XmlCrefAttributeSyntax>();
                        string cref = crefAttribute.ToString().Split('"')[1];

                        if (string.IsNullOrEmpty(cref))
                        {
                            context.ReportDiagnostic(IncorrectExceptionDocumentationDiagnostic.CreateDiagnostic(crefAttribute.GetLocation(), "No exception type referenced."));
                            continue;
                        }

                        if (!cref.Contains("Exception"))
                        {
                            context.ReportDiagnostic(IncorrectExceptionDocumentationDiagnostic.CreateDiagnostic(crefAttribute.Cref.GetLocation(), $"'{cref}' is not an exception type."));
                            continue;
                        }

                        if (semanticModel.GetSymbolInfo(crefAttribute.Cref).Symbol?.ToString() is string exceptionType)
                        {
                            if (documentedExceptions.Any(e => e.Exception == exceptionType))
                                context.ReportDiagnostic(IncorrectExceptionDocumentationDiagnostic.CreateDiagnostic(xmlNode.GetLocation(), $"'{exceptionType}' is already documented."));
                            else
                                documentedExceptions.Add(new ExceptionLocation(exceptionType, xmlNode.GetLocation()));
                        }

                        break;
                    }
                }
            }

            return documentedExceptions;
        }

        private class ExceptionLocation
        {
            public ExceptionLocation(string exception, Location location)
            {
                Exception = exception;
                Location = location;
            }

            public string Exception { get; }
            public Location Location { get; }
            public TextSpan Span
            {
                get => Location.SourceSpan;
            }

            public bool OverlapsWith(ExceptionLocation other)
            {
                return Exception == other.Exception && Span.IntersectsWith(other.Span);
            }

            public override string ToString()
            {
                return $"{Span}: {Exception}";
            }
        }

        //private List<Tuple<string, Location>> GetThrownExceptions(SemanticModel semanticModel, MethodDeclarationSyntax methodDeclaration)
        //{
        //    List<Tuple<string, Location>> thrownExceptions = new List<Tuple<string, Location>>();

        //    List<ArgumentListSyntax> argumentLists = methodDeclaration.GetExpressions<ObjectCreationExpressionSyntax>()
        //        .Select(e => e.ArgumentList)
        //        .ToList();

        //    argumentLists.AddRange(methodDeclaration.GetExpressions<InvocationExpressionSyntax>()
        //        .Select(e => e.ArgumentList));

        //    foreach (ArgumentListSyntax argumentList in argumentLists)
        //    {
        //        ISymbol symbol = semanticModel.GetSymbol(argumentList.Parent);

        //        if(symbol == null)
        //            continue;

        //        try
        //        {
        //            XDocument documentation = XDocument.Parse(symbol.GetDocumentationCommentXml());

        //            thrownExceptions.AddRange(documentation.Descendants("exception")
        //                .Select(e => e.Attribute("cref")?.Value.Substring(2))
        //                .Where(e => !string.IsNullOrEmpty(e))
        //                .Distinct()
        //                .Select(e => new Tuple<string, Location>(e, argumentList.Parent.GetLocation())));
        //        }
        //        catch
        //        {
        //            // Could not load symbol documentation.
        //        }
        //    }

        //    return thrownExceptions;
        //}
    }
}