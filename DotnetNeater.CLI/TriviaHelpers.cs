using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI
{
    public static class TriviaHelpers
    {
        public static SyntaxTrivia NormaliseSingleLineComment(SyntaxTrivia commentTrivia)
        {
            if (!commentTrivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            {
                throw new ArgumentException(
                    $"Expected syntax trivia of type {SyntaxKind.SingleLineCommentTrivia}, " +
                    $"but received {commentTrivia.Kind()}"
                );
            }

            const string commentIndicator = "//";
            var commentText = commentTrivia.ToString().Substring(commentIndicator.Length).Trim();
            return SyntaxFactory.Comment($"{commentIndicator} {commentText}");
        }
    }
}
