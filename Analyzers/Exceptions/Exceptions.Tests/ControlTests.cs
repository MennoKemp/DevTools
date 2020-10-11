using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = DevTools.Tests.Verifiers.CSharpCodeFixVerifier<
    DevTools.Analyzers.Exceptions.ExceptionsAnalyzer,
    DevTools.Analyzers.Exceptions.CodeFixes.ExceptionsCodeFixProvider>;

namespace DevTools.Analyzers.Exceptions.Test
{
    [TestClass]
    public class ControlTests
    {
        [TestMethod]
        public async Task TestNoDiagnostics()
        {
            await VerifyCS.VerifyAnalyzerAsync(string.Empty);
        }

        [TestMethod]
        public async Task TestValidException()
        {
            string test = @"
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new ArgumentNullException();
        }
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task TestValidRethrow()
        {
            string test = @"
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
                throw;
            }
        }
    }
}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
