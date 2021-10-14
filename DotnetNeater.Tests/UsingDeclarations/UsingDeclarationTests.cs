using Xunit;

namespace DotnetNeater.Tests.UsingDeclarations
{
    public class UsingDeclarationTests
    {
        [Fact]
        public void TestCase1()
        {
            var input = TestHelpers.ReadFileAsString("UsingDeclarations/Input 1.txt");
            var output = TestHelpers.ReadFileAsString("UsingDeclarations/Output 1.txt");

            var formatted = TestHelpers.FormatCode(30, input);

            TestHelpers.AssertEqualIgnoringLineEndings(output, formatted);
        }

        [Fact]
        public void TestCase2()
        {
            var input = TestHelpers.ReadFileAsString("UsingDeclarations/Input 2.txt");
            var output = TestHelpers.ReadFileAsString("UsingDeclarations/Output 2.txt");

            var formatted = TestHelpers.FormatCode(30, input);

            TestHelpers.AssertEqualIgnoringLineEndings(output, formatted);
        }
    }
}
