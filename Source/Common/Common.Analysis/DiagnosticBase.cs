using System;
using Microsoft.CodeAnalysis;

namespace DevTools.Analysis
{
    public abstract class DiagnosticBase
    {
        protected DiagnosticBase(string category)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }

        public string Category { get; }
        public abstract string Id { get; }
        public abstract LocalizableString Title { get; }
        public abstract LocalizableString MessageFormat { get; }
        public abstract LocalizableString Description { get; }
        public virtual string HelpLink { get; } = string.Empty;

        public DiagnosticDescriptor Rule
        {
            get => string.IsNullOrEmpty(HelpLink)
                ? new DiagnosticDescriptor(
                    Id,
                    Title,
                    MessageFormat,
                    Category,
                    DiagnosticSeverity.Warning,
                    true,
                    Description)
                : new DiagnosticDescriptor(
                    Id,
                    Title,
                    MessageFormat,
                    Category,
                    DiagnosticSeverity.Warning,
                    true,
                    Description,
                    HelpLink);
        }

        public virtual Diagnostic CreateDiagnostic(Location location, params object[] arguments)
        {
            return Diagnostic.Create(Rule, location, arguments);
        }
    }
}