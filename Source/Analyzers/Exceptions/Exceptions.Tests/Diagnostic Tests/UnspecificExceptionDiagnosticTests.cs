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
    public class UnspecificExceptionDiagnosticTests
    {
        [TestMethod]
        public async Task TestException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new Exception();
        }
    }
}";

        DiagnosticDescriptor rule = new UnspecificExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(10, 19)
                .WithSeverity(rule.DefaultSeverity)
                .WithArguments(typeof(Exception).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestSystemException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new SystemException();
        }
    }
}";

            DiagnosticDescriptor rule = new UnspecificExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(10, 19)
                .WithSeverity(rule.DefaultSeverity)
                .WithArguments(typeof(SystemException).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestNullCoalescingOperator()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = """";
            string b = a
                ?? throw new Exception();
        }
    }
}";

            DiagnosticDescriptor rule = new UnspecificExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(12, 26)
                .WithSeverity(rule.DefaultSeverity)
                .WithArguments(typeof(Exception).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestConditionalOperatorExpression()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = """";
            throw string.IsNullOrEmpty(a)
                ? new Exception()
                : new SystemException();
        }
    }
}";
            
            DiagnosticDescriptor rule = new UnspecificExceptionDiagnostic().Rule;

            DiagnosticResult expected1 = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(12, 19)
                .WithSeverity(rule.DefaultSeverity)
                .WithArguments(typeof(Exception).FullName);

            DiagnosticResult expected2 = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(13, 19)
                .WithSeverity(rule.DefaultSeverity)
                .WithArguments(typeof(SystemException).FullName);

            await VerifyCS.VerifyAnalyzerAsync(code, expected1, expected2);
        }
    }
}