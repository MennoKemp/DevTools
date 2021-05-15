using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = DevTools.Tests.Verifiers.CSharpCodeFixVerifier<
    DevTools.Analyzers.Documentation.DocumentationAnalyzer,
    DevTools.Analyzers.Documentation.CodeFixes.DocumentationCodeFixProvider>;

namespace DevTools.Analyzers.Documentation.Tests
{
    [TestClass]
    public class ControlTest
    {
        [TestMethod]
        public async Task TestNoDiagnostics()
        {
            await VerifyCS.VerifyAnalyzerAsync(string.Empty);
        }

        [TestMethod]
        public async Task TestValidDocumentation()
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
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""a""/> is <see langowrd=""null""/>.</exception>
        static void Main(string[] args)
        {
            if(args == null)
                throw new ArgumentNullException();
        }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(code);
        }
    }
}
