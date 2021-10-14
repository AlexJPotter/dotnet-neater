using DotnetNeater.CLI.Printer;
using Xunit;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests.Operators
{
    public class LineSuffixBoundaryOperatorTests
    {
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
    }
}
