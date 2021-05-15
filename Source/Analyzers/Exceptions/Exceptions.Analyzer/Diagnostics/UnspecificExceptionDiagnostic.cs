using System;
using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Exceptions
{
    public class UnspecificExceptionDiagnostic : DiagnosticBase
    {
        public UnspecificExceptionDiagnostic()
            : base(ExceptionsAnalyzer.Category)
        {
        }

        public override string Id { get; } = "UnspecificException";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.UnspecificExceptionDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.UnspecificExceptionDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.UnspecificExceptionDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
        public override string HelpLink { get; } = @"https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions#things-to-avoid-when-throwing-exceptions";

        public static Type[] UnspecificExceptions { get; } =
        {
            typeof(Exception),
            typeof(SystemException)
        };
    }
}
