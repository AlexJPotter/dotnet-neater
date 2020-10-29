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

            var newNode = RemoveAllTrailingWhitespace(rootNode);

            var outputFileContents = newNode.GetText().ToString();
            Console.Write("OUTPUT:\r\n" + "--------------------\r\n" + outputFileContents.WithVisibleWhitespace() + "--------------------\r\n\r\n");
        }

        private static CSharpSyntaxNode RemoveAllTrailingWhitespace(CSharpSyntaxNode node)
        {
            var returnNode = node;

            for (var tokenIndex = 0; tokenIndex < returnNode.ChildTokens().Count(); tokenIndex++)
            {
                var oldToken = returnNode.ChildTokens().ToList()[tokenIndex];

                var newToken = oldToken;

                var trailingTrivia = newToken.TrailingTrivia;

                var endOfLineTrivia = trailingTrivia.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia))
                    ? trailingTrivia.First(t => t.IsKind(SyntaxKind.EndOfLineTrivia))
                    : (SyntaxTrivia?) null;

                if (endOfLineTrivia != null)
                {
                    newToken = newToken.WithTrailingTrivia(endOfLineTrivia.Value);
                }

                returnNode = returnNode.ReplaceToken(oldToken, newToken);
            }

            for (var nodeIndex = 0; nodeIndex < returnNode.ChildNodes().Count(); nodeIndex++)
            {
                var oldNode = returnNode.ChildNodes().ToList()[nodeIndex];
                var newNode = RemoveAllTrailingWhitespace((CSharpSyntaxNode) oldNode);
                returnNode = returnNode.ReplaceNode(oldNode, newNode);
            }

            return returnNode;
        }
    }
}
