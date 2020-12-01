using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotnetNeater.CLI.Core;
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

            var files = Directory.GetFiles(currentDirectory).Select(filePath => new FileInfo(filePath)).ToList();

            var solutionFiles = files.Where(f => f.Extension == ".sln").ToList();

            if (!solutionFiles.Any())
            {
                await Console.Error.WriteLineAsync("Could not find a solution file");
                return;
            }

            if (solutionFiles.Count > 1)
            {
                await Console.Error.WriteLineAsync("Found multiple solution files");
                return;
            }

            var solutionFile = SolutionFile.Parse(solutionFiles.Single().FullName);

            var projects = solutionFile.ProjectsInOrder.ToList();

            // TODO - Do this more intelligently
            var ignoredDirectories = new[] { "\\bin\\", "\\obj\\" };

            var filesToFormat = projects
                .SelectMany(project =>
                {
                    var projectDirectory = Directory.GetParent(project.AbsolutePath);

                    var cSharpFiles = projectDirectory.EnumerateFiles("*.cs", SearchOption.AllDirectories)
                        .Where(f => !ignoredDirectories.Any(d => f.FullName.Contains(d)))
                        .ToList();

                    return cSharpFiles;
                })
                .Select(fileInfo => fileInfo.FullName)
                .Distinct()
                .ToList();

            Console.WriteLine($"Found {filesToFormat.Count} C# files to format ...\r\n");

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

            var operationRepresentation = SyntaxTreeParser.GetOperationRepresentation(oldRootNode);
            var documentRepresentation = DocumentParser.ParseOperation(operationRepresentation);
            var prettyPrinted = PrintHelpers.Pretty(30, documentRepresentation);

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
