using DotnetNeater.CLI.Operations;
using DotnetNeater.CLI.Printer;
using Xunit;
using Xunit.Abstractions;

namespace DotnetNeater.Tests
{
    public class Example
    {
        private readonly ITestOutputHelper testOutputHelper;

        public Example(ITestOutputHelper outputHelper)
        {
            testOutputHelper = outputHelper;
        }

        [Fact]
        public void Example1()
        {
            string[] names;

            // Let's say we're formatting the following code:
            names = new[] { "Alex", "Martin", "Matt", "Harry", "Max", };

            // We could format it as above, or as follows
            names = new[]
            {
                "Alex",
                "Martin",
                "Matt",
                "Harry",
                "Max",
            };

            // How do we express our intention?

            // To start with, it's clear that we want to put all the names on one line if we can, but if they don't fit
            // we want a line break between each one. We can do this by expressing each string name using the `Text`
            // Operator, expressing our intention to have a (potential) line break between each using the `Line`
            // Operator, joining them with the `Concat` Operator (the native + operator is overloaded for this), and
            // finally using the `Group` Operator to express that we want to keep them on the same line if possible:
            var rootOperation = Operator.Group(
                Operator.Text("names = new[]") + Operator.Line() +
                Operator.Text("{") +
                Operator.Nest(
                    4,
                    Operator.Line() +
                    Operator.Group(
                        Operator.Text(@"""Alex"",") + Operator.Line() + // Why aren't these lines counting?
                        Operator.Text(@"""Martin"",") + Operator.Line() +
                        Operator.Text(@"""Matt"",") + Operator.Line() +
                        Operator.Text(@"""Harry"",") + Operator.Line() +
                        Operator.Text(@"""Max"",")
                    )
                ) + Operator.Line() +
                Operator.Text("};")
            );

            // The full expression we're formatting is about 60 characters long, so if we print with a desired line
            // length of 60 it all gets put on one line:
            var printer = Printer.WithPreferredLineLength(60);
            var result = printer.Print(rootOperation);
            testOutputHelper.WriteLine("Result 1:");
            testOutputHelper.WriteLine(result);
            testOutputHelper.WriteLine("");
            var expected =
@"names = new[] { ""Alex"", ""Martin"", ""Matt"", ""Harry"", ""Max"", };";
            Assert.Equal(expected, result);

            // However, if we choose a smaller number it breaks over multiple lines:
            printer = Printer.WithPreferredLineLength(59);
            result = printer.Print(rootOperation);
            expected =
@"names = new[]
{
    ""Alex"", ""Martin"", ""Matt"", ""Harry"", ""Max"",
};";
            testOutputHelper.WriteLine("Result 2:");
            testOutputHelper.WriteLine(result);
            testOutputHelper.WriteLine("");
            Assert.Equal(expected, result);

            // To fit the names all on one line, there must be at least 45 characters of available space, so if we go
            // slightly below that, they break onto multiple lines:
            printer = Printer.WithPreferredLineLength(44);
            result = printer.Print(rootOperation);
            expected =
                @"names = new[]
{
    ""Alex"",
    ""Martin"",
    ""Matt"",
    ""Harry"",
    ""Max"",
};";
            testOutputHelper.WriteLine("Result 3:");
            testOutputHelper.WriteLine(result);
            testOutputHelper.WriteLine("");
            Assert.Equal(expected, result);
        }
    }
}
