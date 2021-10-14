using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class LineOperatorTests
    {
        [Fact]
        public void Line_PrintsNewLine_IfNotGrouped()
        {
            var rootOperation = Text("Hello,") + Line() + Text("world!");

            var printer = Printer.WithPreferredLineLength(30);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello,\r\nworld!", result);
        }

        [Fact]
        public void Line_PrintsNewLine_IfGrouped_AndContentWillNotFitOnOneLine()
        {
            var rootOperation = Group(Text("Hello,") + Line() + Text("world!"));

            var printer = Printer.WithPreferredLineLength(5);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello,\r\nworld!", result);
        }

        [Fact]
        public void Line_PrintsSpaceInsteadOfNewLine_IfGrouped_AndContentWillFitOnOneLine()
        {
            var rootOperation = Group(Text("Hello,") + Line() + Text("world!"));

            var printer = Printer.WithPreferredLineLength(30);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }
    }
}
