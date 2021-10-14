using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Core.Operator;

namespace DotnetNeater.Tests
{
    public class OperatorTests
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

        [Fact]
        public void Concat_ConcatenatesText()
        {
            var rootOperation = Concat(Text("Hello"), Text(", world!"));

            var printer = Printer.WithPreferredLineLength(5);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }

        [Fact]
        public void Line_PrintsNewLine_IfNotGrouped()
        {
            var rootOperation = Text("Hello,") + Line() + Text("world!");

            var printer = Printer.WithPreferredLineLength(30);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello,\nworld!", result);
        }

        [Fact]
        public void Line_PrintsNewLine_IfGrouped_AndContentWillNotFitOnOneLine()
        {
            var rootOperation = Group(Text("Hello,") + Line() + Text("world!"));

            var printer = Printer.WithPreferredLineLength(5);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello,\nworld!", result);
        }

        [Fact]
        public void Line_PrintsSpaceInsteadOfNewLine_IfGrouped_AndContentWillFitOnOneLine()
        {
            var rootOperation = Group(Text("Hello,") + Line() + Text("world!"));

            var printer = Printer.WithPreferredLineLength(30);
            var result = printer.Print(rootOperation);

            Assert.Equal("Hello, world!", result);
        }

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

        [Fact]
        public void LineSuffix_MovesLineSuffixContentToTheEndOfTheLine()
        {
            var rootOperation = Text("One") + LineSuffix(Text(" // 1")) + Text(" Two");

            var printer = Printer.WithPreferredLineLength(10);
            var result = printer.Print(rootOperation);

            Assert.Equal("One Two // 1", result);
        }

        [Fact]
        public void LineSuffix_ConcatenatesLineSuffixContentAtTheEndOfTheLine()
        {
            var rootOperation =
                Text("One") + LineSuffix(Text(" // 1")) +
                Text(" Two") + LineSuffix(Text(", 2")) +
                Text(" Three") + LineSuffix(Text(", 3"));

            var printer = Printer.WithPreferredLineLength(10);
            var result = printer.Print(rootOperation);

            Assert.Equal("One Two Three // 1, 2, 3", result);
        }

        [Fact]
        public void LineSuffixBoundary_PrintsNewLineThatBreaksUpConcatenationOfLineSuffixContent()
        {
            var rootOperation =
                Text("One") + LineSuffix(Text(" // 1")) +
                Text(" Two") + LineSuffix(Text(", 2")) + LineSuffixBoundary() +
                Text("Three") + LineSuffix(Text(" // 3"));

            var printer = Printer.WithPreferredLineLength(10);
            var result = printer.Print(rootOperation);

            Assert.Equal("One Two // 1, 2\nThree // 3", result);
        }

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

            Assert.Equal("\n  Test", result);
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

            Assert.Equal("\n  Test", result);
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

            Assert.Equal("One\n  Two\n    Three", result);
        }
    }
}
