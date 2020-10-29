using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace DotnetNeater.CLI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            const string inputFilePath = "C:/workspace/innovation/SampleApplication/SampleApplication/Program.cs";

            var inputFileContents = await File.ReadAllTextAsync(inputFilePath);
            Console.Write("INPUT:\r\n" + "--------------------\r\n" + inputFileContents.WithVisibleWhitespace() + "--------------------" + "\r\n\r\n\r\n");

            var syntaxTree = CSharpSyntaxTree.ParseText(inputFileContents);
            var rootNode = (CSharpSyntaxNode) await syntaxTree.GetRootAsync();

            var rulesToApply = new[]
            {
                new TrimTrailingWhitespaceRule(),
            };

            rootNode = rulesToApply.Aggregate(rootNode, (current, rule) => rule.ApplyToNode(current));

            var outputFileContents = rootNode.GetText().ToString();
            Console.Write("OUTPUT:\r\n" + "--------------------\r\n" + outputFileContents.WithVisibleWhitespace() + "--------------------\r\n\r\n");
        }
    }
}
