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
    public class ReservedExceptionCodeFixTest
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
            throw new IndexOutOfRangeException();
        }
    }
}";

            string fixedCode = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new ArgumentNullException();
            throw new ArgumentOutOfRangeException();
        }
    }
}";
            ReservedExceptionDiagnostic diagnostic = new ReservedExceptionDiagnostic();
            DiagnosticDescriptor rule = diagnostic.Rule;

            string replacement1 = string.Format(diagnostic.ReplacementFormat.ToString(), typeof(ArgumentNullException).FullName);

            DiagnosticResult expected1 = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(10, 19)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(typeof(NullReferenceException).FullName, replacement1);

            string replacement2 = string.Format(diagnostic.ReplacementFormat.ToString(), typeof(ArgumentOutOfRangeException).FullName);

            DiagnosticResult expected2 = VerifyCS.Diagnostic(rule.Id)
                .WithLocation(11, 19)
                .WithSeverity(DiagnosticSeverity.Warning)
                .WithArguments(typeof(IndexOutOfRangeException).FullName, replacement2);

            await VerifyCS.VerifyCodeFixAsync(code, new[]{ expected1, expected2 }, fixedCode);
        }
    }
}
