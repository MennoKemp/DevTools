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
    public class ReservedExceptionDiagnosticTest
    {
        [TestMethod]
        public async Task TestNullReferenceException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new NullReferenceException();
        }
    }
}";

            ReservedExceptionDiagnostic diagnostic = new ReservedExceptionDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            string replacement = string.Format(diagnostic.ReplacementFormat.ToString(), typeof(ArgumentNullException).FullName);

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(10, 19)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(typeof(NullReferenceException).FullName, replacement);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestIndexOutOfRangeException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new IndexOutOfRangeException();
        }
    }
}";

            ReservedExceptionDiagnostic diagnostic = new ReservedExceptionDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            string replacement = string.Format(diagnostic.ReplacementFormat.ToString(), typeof(ArgumentOutOfRangeException).FullName);

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
               .WithLocation(10, 19)
               .WithSeverity(DiagnosticSeverity.Warning)
               .WithArguments(typeof(IndexOutOfRangeException).FullName, replacement);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestDivideByZeroException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new DivideByZeroException();
        }
    }
}";

            DiagnosticDescriptor rule = new ReservedExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
               .WithLocation(10, 19)
               .WithSeverity(DiagnosticSeverity.Warning)
               .WithArguments(typeof(DivideByZeroException).FullName, string.Empty);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }

        [TestMethod]
        public async Task TestInvalidCastException()
        {
            string code = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new InvalidCastException();
        }
    }
}";

            DiagnosticDescriptor rule = new ReservedExceptionDiagnostic().Rule;

            DiagnosticResult expected = VerifyCS.Diagnostic(rule.Id)
               .WithLocation(10, 19)
               .WithSeverity(DiagnosticSeverity.Warning)
               .WithArguments(typeof(InvalidCastException).FullName, string.Empty);

            await VerifyCS.VerifyAnalyzerAsync(code, expected);
        }
    }
}
