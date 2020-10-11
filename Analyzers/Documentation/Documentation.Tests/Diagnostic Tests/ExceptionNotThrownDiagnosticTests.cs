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
    public class ExceptionNotDocumentedDiagnosticTests
    {
        [TestMethod]
        public async Task TestExceptionNotDocumented()
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
        /// <exception cref=""NotImplementedException""></exception>
        static void Main(string[] args)
        {
            if(args == null)
                throw new ArgumentNullException();

            throw new NotImplementedException();
        }
    }
}";
            ExceptionNotDocumentedDiagnostic diagnostic = new ExceptionNotDocumentedDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(15, 23)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(typeof(ArgumentNullException).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }
    }
}
