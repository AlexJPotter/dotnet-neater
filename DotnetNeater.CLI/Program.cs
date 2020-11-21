using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI
{
    public static class Program
    {
        private static readonly IEnumerable<IFormattingRule> RulesToApply = new[]
        {
            new TrimTrailingWhitespaceRule(),
        };

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

            var oldSyntaxTree = CSharpSyntaxTree.ParseText(oldFileContents);
            var oldRootNode = (CSharpSyntaxNode) await oldSyntaxTree.GetRootAsync();

            var newRootNode = RulesToApply.Aggregate(oldRootNode, (current, rule) => rule.ApplyToNode(current));
            var newSyntaxTree = CSharpSyntaxTree.Create(newRootNode);

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
