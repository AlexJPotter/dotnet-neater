using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotnetNeater.CLI.Exceptions;
using Microsoft.Build.Construction;

namespace DotnetNeater.CLI.Helpers
{
    public static class FileHelpers
    {
        private const string SolutionFileExtension = ".sln";

        public static ICollection<string> DiscoverFilesToFormat(string directoryPath)
        {
            var projects = DiscoverProjects();
            var ignoredDirectories = GetIgnoredDirectories();
            var filesToFormat = DetermineFilesToFormat();

            Console.WriteLine($"Found {filesToFormat.Count} C# files to format ...\r\n");

            return filesToFormat;

            ICollection<ProjectInSolution> DiscoverProjects()
            {
                var allFiles =
                    Directory.GetFiles(directoryPath)
                        .Select(filePath => new FileInfo(filePath))
                        .ToList();

                var solutionFiles = allFiles.Where(f => f.Extension == SolutionFileExtension).ToList();

                if (!solutionFiles.Any())
                {
                    throw new CodeDiscoveryException("Could not find a solution file");
                }
                if (solutionFiles.Count > 1)
                {
                    throw new CodeDiscoveryException("Found multiple solution files");
                }

                var solutionFile = SolutionFile.Parse(solutionFiles.Single().FullName);

                return solutionFile.ProjectsInOrder.ToList();
            }

            static ICollection<string> GetIgnoredDirectories()
            {
                // TODO - Do this more intelligently, including reading from a config file
                return new[]
                {
                    @"\bin\",
                    @"\obj\",
                };
            }

            ICollection<string> DetermineFilesToFormat()
            {
                return projects
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
            }
        }
    }
}
