using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class LineSuffixOperatorTests
    {
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
    }
}
