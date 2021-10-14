using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class HardLineOperatorTests
    {
        [Fact]
        public void HardLine_PrintsNewLine_IfNotGrouped()
        {
            var rootOperation = Text("employees") + HardLine() + Text(".Select(x => x.Name);");

            var printer = Printer.WithPreferredLineLength(100);
            var result = printer.Print(rootOperation);

            Assert.Equal("employees\n.Select(x => x.Name);", result);
        }

        [Fact]
        public void HardLine_PrintsNewLine_IfGrouped_AndContentWillFitOnOneLine()
        {
            var rootOperation = Group(Text("employees") + HardLine() + Text(".Select(x => x.Name);"));

            var printer = Printer.WithPreferredLineLength(100);
            var result = printer.Print(rootOperation);

            Assert.Equal("employees\n.Select(x => x.Name);", result);
        }

        [Fact]
        public void HardLine_PrintsNewLine_IfGrouped_AndContentWillNotFitOnOneLine()
        {
            var rootOperation = Group(Text("employees") + HardLine() + Text(".Select(x => x.Name);"));

            var printer = Printer.WithPreferredLineLength(10);
            var result = printer.Print(rootOperation);

            Assert.Equal("employees\n.Select(x => x.Name);", result);
        }
    }
}
