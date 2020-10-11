using System;
using System.Collections.Immutable;
using System.Linq;
using DevTools.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DevTools.Analyzers.Exceptions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExceptionsAnalyzer : DiagnosticAnalyzer
    {
        internal const string Category = "Exceptions";

        public static UnspecificExceptionDiagnostic UnspecificExceptionDiagnostic { get; } = new UnspecificExceptionDiagnostic();
        public static ReservedExceptionDiagnostic ReservedExceptionDiagnostic { get; } = new ReservedExceptionDiagnostic();
        public static VariableExceptionDiagnostic VariableExceptionDiagnostic { get; } = new VariableExceptionDiagnostic();
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.CreateRange(new[]
        {
            UnspecificExceptionDiagnostic.Rule,
            ReservedExceptionDiagnostic.Rule,
            VariableExceptionDiagnostic.Rule
        });

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ThrowStatement);
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ThrowExpression);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            foreach (SyntaxNode exceptionSource in context.Node.GetExceptionSources())
            {
                SyntaxKind exceptionSourceKind = exceptionSource.Kind();

                if (exceptionSourceKind == SyntaxKind.ObjectCreationExpression)
                    AnalyzeExceptionCreation(context, exceptionSource as ObjectCreationExpressionSyntax);
                else if (exceptionSourceKind == SyntaxKind.IdentifierName)
                    context.ReportDiagnostic(VariableExceptionDiagnostic.CreateDiagnostic(exceptionSource.GetLocation()));
            }
        }

        private static void AnalyzeExceptionCreation(SyntaxNodeAnalysisContext context, ObjectCreationExpressionSyntax exceptionCreationExpression)
        {
            TypeInfo typeInfo = context.SemanticModel.GetTypeInfo(exceptionCreationExpression);

            if (UnspecificExceptionDiagnostic.UnspecificExceptions.FirstOrDefault(e => typeInfo.EqualsType(e)) is Type unspecificExceptionType)
            {
                context.ReportDiagnostic(UnspecificExceptionDiagnostic.CreateDiagnostic(exceptionCreationExpression.GetLocation(), unspecificExceptionType.FullName));
            }
            else if (ReservedExceptionDiagnostic.ReservedExceptions.FirstOrDefault(e => typeInfo.EqualsType(e)) is Type exceptionType)
            {
                context.ReportDiagnostic(ReservedExceptionDiagnostic.ExceptionReplacements.TryGetValue(exceptionType, out Type replacement)
                    ? ReservedExceptionDiagnostic.CreateDiagnostic(exceptionCreationExpression.GetLocation(), exceptionType.FullName, replacement.FullName)
                    : ReservedExceptionDiagnostic.CreateDiagnostic(exceptionCreationExpression.GetLocation(), exceptionType.FullName));
            }
        }
    }
}