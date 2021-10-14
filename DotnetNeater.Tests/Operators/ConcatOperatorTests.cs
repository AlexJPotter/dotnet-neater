using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class ConcatOperatorTests
    {
        [Fact]
        public void Concat_ConcatenatesText()
        {
            var rootOperation = Concat(Text("Hello"), Text(", world!"));

            var printer = Printer.WithPreferredLineLength(5);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }
    }
}
