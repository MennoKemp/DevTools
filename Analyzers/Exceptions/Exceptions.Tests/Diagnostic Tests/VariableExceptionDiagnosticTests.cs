using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = DevTools.Tests.Verifiers.CSharpCodeFixVerifier<
    DevTools.Analyzers.Exceptions.ExceptionsAnalyzer,
    DevTools.Analyzers.Exceptions.CodeFixes.ExceptionsCodeFixProvider>;

namespace DevTools.Analyzers.Exceptions.Test
{
    [TestClass]
    public class VariableExceptionDiagnosticTests
    {
        [TestMethod]
        public async Task TestVariableException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Exception exception = new Exception();
            throw exception;
        }
    }
}";

            DiagnosticDescriptor rule = new VariableExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 19)
                .WithSeverity(rule.DefaultSeverity);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestInvalidRethrow()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}";

            DiagnosticDescriptor rule = new VariableExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(15, 23)
                .WithSeverity(rule.DefaultSeverity)
                .WithArguments(typeof(Exception).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }
    }
}
