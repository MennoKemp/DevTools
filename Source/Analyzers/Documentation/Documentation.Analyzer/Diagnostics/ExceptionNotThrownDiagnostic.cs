using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Documentation
{
    public class ExceptionNotThrownDiagnostic : DiagnosticBase
    {
        public ExceptionNotThrownDiagnostic()
            : base(DocumentationAnalyzer.Category)
        {
        }

        public override string Id { get; } = "ExceptionNotThrown";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ExceptionNotThrownDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ExceptionNotThrownDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ExceptionNotThrownDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
    }
}
