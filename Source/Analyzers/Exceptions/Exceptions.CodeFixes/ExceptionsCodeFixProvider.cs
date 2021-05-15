using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DevTools.Analyzers.Exceptions.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExceptionsCodeFixProvider)), Shared]
    public class ExceptionsCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(
            ExceptionsAnalyzer.ReservedExceptionDiagnostic.Id);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            Diagnostic diagnostic = context.Diagnostics.First();
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            if (diagnostic.Id == ExceptionsAnalyzer.ReservedExceptionDiagnostic.Id)
            {
                ObjectCreationExpressionSyntax exceptionCreation = (ObjectCreationExpressionSyntax)root.FindNode(diagnosticSpan);
                string exceptionName = exceptionCreation.Type.ToString();

                Type exceptionType = ReservedExceptionDiagnostic.ExceptionReplacements.Keys.FirstOrDefault(e => e.Name.Equals(exceptionName));
                if(exceptionType == null)
                    return;

                if(!ReservedExceptionDiagnostic.ExceptionReplacements.TryGetValue(exceptionType, out Type replacementType))
                    return;

                context.RegisterCodeFix(
                    CodeAction.Create(
                        CodeFixResources.ReservedCodeFixTitle,
                        c => ReplaceReservedException(context.Document, exceptionCreation, replacementType, c),
                        nameof(CodeFixResources.ReservedCodeFixTitle)), 
                    diagnostic);
            }
        }

        private static async Task<Document> ReplaceReservedException(Document document, ObjectCreationExpressionSyntax exceptionCreation, MemberInfo replacementExceptionType, CancellationToken cancellationToken)
        {
            TypeSyntax replacementExceptionTypeSyntax = SyntaxFactory.ParseTypeName(replacementExceptionType.Name);

            ArgumentListSyntax arguments = exceptionCreation.ArgumentList;
            arguments.Update(arguments.OpenParenToken, new SeparatedSyntaxList<ArgumentSyntax>(), arguments.CloseParenToken);

            ObjectCreationExpressionSyntax newExceptionCreation = SyntaxFactory.ObjectCreationExpression(
                exceptionCreation.NewKeyword,
                replacementExceptionTypeSyntax,
                SyntaxFactory.ArgumentList(),
                null);

            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken);
            SyntaxNode newRoot = root.ReplaceNode(exceptionCreation, newExceptionCreation);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
