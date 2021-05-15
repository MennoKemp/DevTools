using System;
using System.Collections.Generic;
using DevTools.Analysis;
using Microsoft.CodeAnalysis;

namespace DevTools.Analyzers.Exceptions
{
    public class ReservedExceptionDiagnostic : DiagnosticBase
    {
        public ReservedExceptionDiagnostic() 
            : base(ExceptionsAnalyzer.Category)
        {
        }

        public override string Id { get; } = "ReservedException";
        public override LocalizableString Title { get; } = new LocalizableResourceString(nameof(Resources.ReservedExceptionDiagnosticTitle), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString MessageFormat { get; } = new LocalizableResourceString(nameof(Resources.ReservedExceptionDiagnosticMessageFormat), Resources.ResourceManager, typeof(Resources));
        public LocalizableString ReplacementFormat { get; } = new LocalizableResourceString(nameof(Resources.ReservedExceptionDiagnosticReplacementFormat), Resources.ResourceManager, typeof(Resources));
        public override LocalizableString Description { get; } = new LocalizableResourceString(nameof(Resources.ReservedExceptionDiagnosticDescription), Resources.ResourceManager, typeof(Resources));
        public override string HelpLink { get; } = @"https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions#things-to-avoid-when-throwing-exceptions";

        public static Type[] ReservedExceptions { get; } =
        {
            typeof(NullReferenceException),
            typeof(IndexOutOfRangeException),
            typeof(DivideByZeroException),
            typeof(InvalidCastException)
        };

        public static Dictionary<Type, Type> ExceptionReplacements { get; } = new Dictionary<Type, Type>
        {
            { typeof(NullReferenceException), typeof(ArgumentNullException) },
            { typeof(IndexOutOfRangeException), typeof(ArgumentOutOfRangeException) }
        };
        
        public override Diagnostic CreateDiagnostic(Location location, params object[] arguments)
        {
            return arguments.Length == 2
                ? Diagnostic.Create(Rule, location, arguments[0], string.Format(ReplacementFormat.ToString(), arguments[1]))
                : Diagnostic.Create(Rule, location, arguments[0], string.Empty);
        }
    }
}