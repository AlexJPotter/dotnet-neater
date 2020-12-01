using System;
using System.Linq;
using DotnetNeater.CLI.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI.Helpers
{
    public static class TriviaHelpers
    {
        public static SyntaxTrivia TrimTrailingWhitespaceFromComment(SyntaxTrivia commentTrivia)
        {
            if (!commentTrivia.IsAnyComment())
            {
                throw new ArgumentException(
                    $"Expected syntax trivia of type " +
                    $"{SyntaxKind.SingleLineCommentTrivia} or {SyntaxKind.MultiLineCommentTrivia}, " +
                    $"but received {commentTrivia.Kind()}"
                );
            }

            return commentTrivia.IsSingleLineComment()
                ? TrimTrailingWhitespaceFromSingleLineComment(commentTrivia)
                : TrimTrailingWhitespaceFromMultiLineComment(commentTrivia);
        }

        private static SyntaxTrivia TrimTrailingWhitespaceFromSingleLineComment(SyntaxTrivia commentTrivia)
        {
            const string commentIndicator = "//";
            var commentText = commentTrivia.ToString().Substring(commentIndicator.Length).TrimEnd();
            return SyntaxFactory.Comment($"{commentIndicator}{commentText}");
        }

        private static SyntaxTrivia TrimTrailingWhitespaceFromMultiLineComment(SyntaxTrivia commentTrivia)
        {
            const string commentStartIndicator = "/*";

            var commentMinusStartIndicator = commentTrivia.ToString().Substring(commentStartIndicator.Length).TrimEnd();

            var commentLines = commentMinusStartIndicator
                .Split(Environment.NewLine) // TODO - Allow new line type to be set in config, or detected from Git settings
                .ToList();

            var normalisedCommentLines = commentLines
                .Select(line => line.TrimEnd())
                .ToList();

            commentMinusStartIndicator = string.Join(Environment.NewLine, normalisedCommentLines);

            return SyntaxFactory.Comment($"{commentStartIndicator}{commentMinusStartIndicator}");
        }
    }
}
