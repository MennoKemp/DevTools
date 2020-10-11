using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Exceptions
{
    public class VariableExceptionDiagnostic : DiagnosticBase
    {
        public VariableExceptionDiagnostic()
            : base(ExceptionsAnalyzer.Category)
        {
        }

        public override string Id { get; } = "VariableException";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.VariableExceptionDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.VariableExceptionDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.VariableExceptionDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
    }
}
