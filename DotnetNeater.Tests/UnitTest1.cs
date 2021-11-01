using DotnetNeater.CLI.Operations;
using Xunit;
using DotnetNeater.CLI.Printer;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var operation =
                Group(
                    Group(
                        Group(
                            Group(Text("hello") + Line() + Text("a"))
                        + Line() + Text("b"))
                    + Line() + Text("c"))
                + Line() + Text("d"));

            Assert.Equal("hello\r\na\r\nb\r\nc\r\nd", Print(5, operation));
            Assert.Equal("hello\r\na\r\nb\r\nc\r\nd", Print(6, operation));
            Assert.Equal("hello a\r\nb\r\nc\r\nd", Print(7, operation));
            Assert.Equal("hello a\r\nb\r\nc\r\nd", Print(8, operation));
            Assert.Equal("hello a b\r\nc\r\nd", Print(9, operation));
            Assert.Equal("hello a b\r\nc\r\nd", Print(10, operation));
            Assert.Equal("hello a b c\r\nd", Print(11, operation));
            Assert.Equal("hello a b c\r\nd", Print(12, operation));
            Assert.Equal("hello a b c d", Print(13, operation));
        }

        [Fact]
        public void Test2()
        {
            var operation =
                Group(
                    Group(
                        Group(
                            Group(Text("hello") + Line() + Text("a"))
                        + SoftLine() + Text("b"))
                    + SoftLine() + Text("c"))
                + SoftLine() + Text("d"));

            Assert.Equal("hello\r\na\r\nb\r\nc\r\nd", Print(5, operation));
            Assert.Equal("hello\r\na\r\nb\r\nc\r\nd", Print(6, operation));
            Assert.Equal("hello a\r\nb\r\nc\r\nd", Print(7, operation));
            Assert.Equal("hello ab\r\nc\r\nd", Print(8, operation));
            Assert.Equal("hello abc\r\nd", Print(9, operation));
            Assert.Equal("hello abcd", Print(10, operation));
        }

        private static string Print(int preferredLineLength, Operation rootOperation)
        {
            var printer = Printer.WithPreferredLineLength(preferredLineLength);
            return printer.Print(rootOperation);
        }
    }
}
