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
    public class IncorrectExceptionDocumentationDiagnosticTests
    {
        [TestMethod]
        public async Task TestIncorrectFormat()
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
        /// <exception></exception>
        /// <exception cref></exception>
        static void Main(string[] args)
        {
        }
    }
}";

            IncorrectExceptionDocumentationDiagnostic diagnostic = new IncorrectExceptionDocumentationDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected1 = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 13)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(@"Xml tag should be of format '<exception cref=""ExceptionType"">'.");

            DiagnosticResult expected2 = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(12, 13)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(@"Xml tag should be of format '<exception cref=""ExceptionType"">'.");

            await VerifyCS.VerifyAnalyzerAsync(code, expected1, expected2);
        }

        [TestMethod]
        public async Task TestMissingExceptionReference()
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
        /// <exception cref=""""></exception>
        static void Main(string[] args)
        {
        }
    }
}";

            IncorrectExceptionDocumentationDiagnostic diagnostic = new IncorrectExceptionDocumentationDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 24)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments("No exception type referenced.");

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestInvalidExceptionReference()
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
        /// <exception cref=""object""></exception>
        static void Main(string[] args)
        {
        }
    }
}";

            IncorrectExceptionDocumentationDiagnostic diagnostic = new IncorrectExceptionDocumentationDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 30)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments("'object' is not an exception type.");

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestDuplicateExceptionDocumentation()
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
        /// <exception cref=""ArgumentNullException""></exception>
        static void Main(string[] args)
        {
            if(args == null)
                throw new ArgumentNullException(nameof(args));
        }
    }
}";

            IncorrectExceptionDocumentationDiagnostic diagnostic = new IncorrectExceptionDocumentationDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(12, 13)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments("'System.ArgumentNullException' is already documented.");

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestIncorrectCaseExceptionTag()
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
        /// <Exception cref=""ArgumentNullException""></exception>
        static void Main(string[] args)
        {
        }
    }
}";

            IncorrectExceptionDocumentationDiagnostic diagnostic = new IncorrectExceptionDocumentationDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 13)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(@"Xml tag should be of format '<exception cref=""ExceptionType"">'.");

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }
    }
}
