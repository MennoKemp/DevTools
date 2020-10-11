using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Documentation
{
    public class IncorrectExceptionDocumentationDiagnostic : DiagnosticBase
    {
        public IncorrectExceptionDocumentationDiagnostic()
            : base(DocumentationAnalyzer.Category)
        {
        }

        public override string Id { get; } = "IncorrectExceptionDocumentation";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.IncorrectExceptionDocumentationDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.IncorrectExceptionDocumentationDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.IncorrectExceptionDocumentationDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
    }
}
