using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class NestOperatorTests
    {
        [Fact]
        public void Nest_HasNoEffect_WithoutNewLines()
        {
            var rootOperation = Nest(2, Text("Hello, world!"));

            var printer = Printer.WithPreferredLineLength(4);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }

        [Fact]
        public void Nest_IfNotGrouped_PrintsIndentationAfterNewLine()
        {
            var rootOperation = Nest(2, Line() + Text("Test"));

            var printer = Printer.WithPreferredLineLength(10);
            var result = printer.Print(rootOperation);

            Assert.Equal("\r\n  Test", result);
        }

        [Fact]
        public void Nest_IfGrouped_AndFitsOnOneLine_DoesNotPrintIndentation()
        {
            var rootOperation = Group(Nest(2, SoftLine() + Text("Test")));

            var printer = Printer.WithPreferredLineLength(100);
            var result = printer.Print(rootOperation);

            Assert.Equal("Test", result);
        }

        [Fact]
        public void Nest_IfGrouped_AndDoesNotFitOnOneLine_PrintsIndentation()
        {
            var rootOperation = Group(Nest(2, SoftLine() + Text("Test")));

            var printer = Printer.WithPreferredLineLength(3);
            var result = printer.Print(rootOperation);

            Assert.Equal("\r\n  Test", result);
        }

        [Fact]
        public void Nest_CanBeStacked()
        {
            var rootOperation = Group(
                Text("One") +
                Nest(2, Line() + Text("Two") +
                        Nest(2, Line() + Text("Three")))
            );

            var printer = Printer.WithPreferredLineLength(5);
            var result = printer.Print(rootOperation);

            Assert.Equal("One\r\n  Two\r\n    Three", result);
        }
    }
}
