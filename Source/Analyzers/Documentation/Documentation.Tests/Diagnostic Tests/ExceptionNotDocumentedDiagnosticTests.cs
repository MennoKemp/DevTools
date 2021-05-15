using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = DevTools.Tests.Verifiers.CSharpCodeFixVerifier<
    DevTools.Analyzers.Documentation.DocumentationAnalyzer,
    DevTools.Analyzers.Documentation.CodeFixes.DocumentationCodeFixProvider>;

namespace DevTools.Analyzers.Documentation.Tests
{
    [TestClass]
    public class ExceptionNotThrownDiagnosticTests
    {
        [TestMethod]
        public async Task TestExceptionNotThrown()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        /// <summary>
        /// Some summary.
        /// </summary>
        /// <exception cref=""ArgumentNullException""></exception>
        /// <exception cref=""NotImplementedException""></exception>
        static void Main(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}";
            ExceptionNotThrownDiagnostic diagnostic = new ExceptionNotThrownDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 13)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(typeof(ArgumentNullException).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }
    }
}
