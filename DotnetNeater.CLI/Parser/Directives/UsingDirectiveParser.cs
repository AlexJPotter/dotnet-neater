using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Directives
{
    public static class UsingDirectiveParser
    {
        public static Operation Parse(UsingDirectiveSyntax usingDirective)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive

            if (usingDirective.Alias != null)
            {
                return
                    Text("using ") +
                    SyntaxTreeParser.Parse(usingDirective.Alias) +
                    SyntaxTreeParser.Parse(usingDirective.Name) +
                    Text(";") +
                    CombineSingleLineCommentsIntoLineSuffix(usingDirective) +
                    Line();
            }

            if (usingDirective.StaticKeyword != default)
            {
                return
                    Text("using static ") +
                    SyntaxTreeParser.Parse(usingDirective.Name) +
                    Text(";") +
                    CombineSingleLineCommentsIntoLineSuffix(usingDirective) +
                    Line();
            }

            return
                Text("using ") +
                SyntaxTreeParser.Parse(usingDirective.Name) +
                Text(";") +
                CombineSingleLineCommentsIntoLineSuffix(usingDirective) +
                Line();
        }

        private static Operation CombineSingleLineCommentsIntoLineSuffix(SyntaxNode syntaxNode)
        {
            var commentText = " //";

            var trivia =
                syntaxNode.DescendantTrivia()
                    .Where(t => t.Kind() == SyntaxKind.SingleLineCommentTrivia)
                    .ToList();

            if (!trivia.Any())
            {
                return Nil();
            }

            foreach (var singleLineCommentTrivia in trivia)
            {
                commentText += " " + singleLineCommentTrivia.ToString()["//".Length..].Trim();
            }

            return LineSuffix(Text(commentText));
        }
    }
}
