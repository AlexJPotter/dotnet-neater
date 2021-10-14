using Xunit;
using DotnetNeater.CLI.Core;
using static DotnetNeater.CLI.Core.Operator;

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

            Assert.Equal("hello\na\nb\nc\nd", PrintHelpers.Print(5, operation));
            Assert.Equal("hello\na\nb\nc\nd", PrintHelpers.Print(6, operation));
            Assert.Equal("hello a\nb\nc\nd", PrintHelpers.Print(7, operation));
            Assert.Equal("hello a\nb\nc\nd", PrintHelpers.Print(8, operation));
            Assert.Equal("hello a b\nc\nd", PrintHelpers.Print(9, operation));
            Assert.Equal("hello a b\nc\nd", PrintHelpers.Print(10, operation));
            Assert.Equal("hello a b c\nd", PrintHelpers.Print(11, operation));
            Assert.Equal("hello a b c\nd", PrintHelpers.Print(12, operation));
            Assert.Equal("hello a b c d", PrintHelpers.Print(13, operation));
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

            Assert.Equal("hello\na\nb\nc\nd", PrintHelpers.Print(5, operation));
            Assert.Equal("hello\na\nb\nc\nd", PrintHelpers.Print(6, operation));
            Assert.Equal("hello a\nb\nc\nd", PrintHelpers.Print(7, operation));
            Assert.Equal("hello ab\nc\nd", PrintHelpers.Print(8, operation));
            Assert.Equal("hello abc\nd", PrintHelpers.Print(9, operation));
            Assert.Equal("hello abcd", PrintHelpers.Print(10, operation));
        }
    }
}
