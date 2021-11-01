using System.IO;
using DotnetNeater.CLI;
using DotnetNeater.CLI.Operations;
using DotnetNeater.CLI.Parser;
using DotnetNeater.CLI.Printer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace DotnetNeater.Tests
{
    public static class TestHelpers
    {
        public static FileStream GetFileStream(string path)
        {
            var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var fileInfo = provider.GetFileInfo(path);
            return new FileStream(path: fileInfo.PhysicalPath, mode: FileMode.Open);
        }

        public static string ReadFileAsString(string path)
        {
            using var stream = GetFileStream(path);
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static Operation GetRootOperation(string code)
        {
            var syntaxTree = (CSharpSyntaxTree) CSharpSyntaxTree.ParseText(code);
            return SyntaxTreeParser.Parse(syntaxTree.GetRoot());
        }

        public static string FormatCode(int preferredLineLength, string code)
        {
            var printer = Printer.WithPreferredLineLength(preferredLineLength);
            return printer.Print(GetRootOperation(code));
        }

        public static void AssertEqualIgnoringLineEndings(string expected, string actual)
        {
            Assert.Equal(NormaliseLineEndings(expected), NormaliseLineEndings(actual));
        }

        public static string NormaliseLineEndings(string value)
        {
            return value.Replace("\r\n", "\n").Replace("\r", "");
        }
    }
}
