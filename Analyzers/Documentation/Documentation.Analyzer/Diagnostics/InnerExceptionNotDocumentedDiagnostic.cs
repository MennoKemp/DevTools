using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Documentation
{
    public class InnerExceptionNotDocumentedDiagnostic : DiagnosticBase
    {
        public InnerExceptionNotDocumentedDiagnostic()
            : base(DocumentationAnalyzer.Category)
        {
        }

        public override string Id { get; } = "InnerExceptionNotDocumented";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.InnerExceptionNotDocumentedDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.InnerExceptionNotDocumentedDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.InnerExceptionNotDocumentedDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
    }
}
