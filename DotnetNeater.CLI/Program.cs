using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotnetNeater.CLI.Helpers;
using DotnetNeater.CLI.Parser;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("*** dotnet-neater ***");
            Console.WriteLine();

            var currentDirectory = Environment.CurrentDirectory;
            var filesToFormat = FileHelpers.DiscoverFilesToFormat(currentDirectory);

            foreach (var filePath in filesToFormat)
            {
                await FormatFile(filePath);
            }

            Console.WriteLine();
        }

        private static async Task FormatFile(string filePath)
        {
            var oldFileContents = await File.ReadAllTextAsync(filePath);

            var oldSyntaxTree = (CSharpSyntaxTree) CSharpSyntaxTree.ParseText(oldFileContents);
            var oldRootNode = await oldSyntaxTree.GetRootAsync();

            var rootOperation = SyntaxTreeParser.Parse(oldRootNode);

            var printer = Printer.Printer.WithPreferredLineLength(30); // TODO - Take preferred line length from config
            var prettyPrinted = printer.Print(rootOperation);

            var newSyntaxTree = (CSharpSyntaxTree) CSharpSyntaxTree.ParseText(prettyPrinted);

            var hasChanged = newSyntaxTree.GetChanges(oldSyntaxTree).Any();

            if (hasChanged)
            {
                var newFileContents = (await newSyntaxTree.GetTextAsync()).ToString();
                await File.WriteAllTextAsync(filePath, newFileContents);
            }

            var originalConsoleColour = Console.ForegroundColor;
            Console.ForegroundColor = hasChanged ? ConsoleColor.White : ConsoleColor.DarkGray;
            Console.WriteLine(filePath);
            Console.ForegroundColor = originalConsoleColour;
        }
    }
}
