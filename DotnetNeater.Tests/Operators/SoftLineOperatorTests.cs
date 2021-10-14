using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class SoftLineOperatorTests
    {
        [Fact]
        public void SoftLine_PrintsNewLine_IfNotGrouped()
        {
            var rootOperation = Text("employees") + SoftLine() + Text(".Select(x => x.Name);");

            var printer = Printer.WithPreferredLineLength(100);
            var result = printer.Print(rootOperation);

            Assert.Equal("employees\n.Select(x => x.Name);", result);
        }

        [Fact]
        public void SoftLine_PrintsNewLine_IfGrouped_AndContentWillNotFitOnOneLine()
        {
            var rootOperation = Group(Text("employees") + SoftLine() + Text(".Select(x => x.Name);"));

            var printer = Printer.WithPreferredLineLength(10);
            var result = printer.Print(rootOperation);

            Assert.Equal("employees\n.Select(x => x.Name);", result);
        }

        [Fact]
        public void SoftLine_PrintsEmptyStringInsteadOfNewLine_IfGrouped_AndContentWillFitOnOneLine()
        {
            var rootOperation = Group(Text("employees") + SoftLine() + Text(".Select(x => x.Name);"));

            var printer = Printer.WithPreferredLineLength(100);
            var result = printer.Print(rootOperation);

            Assert.Equal("employees.Select(x => x.Name);", result);
        }
    }
}
