using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Documentation
{
    public class ExceptionNotDocumentedDiagnostic : DiagnosticBase
    {
        public ExceptionNotDocumentedDiagnostic()
            : base(DocumentationAnalyzer.Category)
        {
        }

        public override string Id { get; } = "ExceptionNotDocumented";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ExceptionNotDocumentedDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ExceptionNotDocumentedDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ExceptionNotDocumentedDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
    }
}
