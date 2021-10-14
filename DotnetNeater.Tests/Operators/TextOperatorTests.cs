using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class TextOperatorTests
    {
        [Fact]
        public void Text_IsPrintedAsIs_WhenItFitsOnOneLine()
        {
            var rootOperation = Text("Hello, world!");

            var printer = Printer.WithPreferredLineLength(100);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }

        [Fact]
        public void Text_IsPrintedAsIs_WhenItDoesNotFitOnOneLine()
        {
            var rootOperation = Text("Hello, world!");

            var printer = Printer.WithPreferredLineLength(5);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }
    }
}
