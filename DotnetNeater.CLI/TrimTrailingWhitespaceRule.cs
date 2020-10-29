using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI
{
    public class TrimTrailingWhitespaceRule : IFormattingRule
    {
        public string Name => "TrimTrailingWhitespace";

        public CSharpSyntaxNode ApplyToNode(CSharpSyntaxNode node)
        {
            for (var tokenIndex = 0; tokenIndex < node.ChildTokens().Count(); tokenIndex++)
            {
                var oldToken = node.ChildTokens().ToList()[tokenIndex];
                var newToken = ApplyToToken(oldToken);
                node = node.ReplaceToken(oldToken, newToken);
            }

            for (var nodeIndex = 0; nodeIndex < node.ChildNodes().Count(); nodeIndex++)
            {
                var oldNode = node.ChildNodes().ToList()[nodeIndex];
                var newNode = ApplyToNode((CSharpSyntaxNode) oldNode);
                node = node.ReplaceNode(oldNode, newNode);
            }

            return node;
        }

        private static SyntaxToken ApplyToToken(SyntaxToken token)
        {
            token = token.WithLeadingTrivia(NormaliseLeadingTrivia(token.LeadingTrivia));
            token = token.WithTrailingTrivia(NormaliseTrailingTrivia(token.TrailingTrivia));
            return token;
        }

        private static SyntaxTriviaList NormaliseLeadingTrivia(SyntaxTriviaList triviaList)
        {
            var triviaLines = SplitByLine(triviaList).ToList();

            var indentWidth = !triviaLines.Any() ? 0 : triviaLines.Last().Sum(t => t.FullSpan.Length);

            return triviaLines
                .SelectMany((triviaLine, lineIndex) => NormaliseTriviaLine(
                    triviaLine: triviaLine,
                    hasLeadingCode: false,
                    indentWidth: indentWidth
                ).ToList())
                .ToSyntaxTriviaList();
        }

        private static SyntaxTriviaList NormaliseTrailingTrivia(SyntaxTriviaList triviaList)
        {
            var triviaLines = SplitByLine(triviaList).ToList();

            return triviaLines
                .SelectMany((triviaLine, lineIndex) => NormaliseTriviaLine(
                    triviaLine: triviaLine,
                    hasLeadingCode: lineIndex == 0,
                    indentWidth: 0
                ).ToList())
                .ToSyntaxTriviaList();
        }

        private static SyntaxTriviaList NormaliseTriviaLine(SyntaxTriviaList triviaLine, bool hasLeadingCode, int indentWidth)
        {
            var endOfLineCount = triviaLine.Count(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            // If the trivia doesn't run to the end of the line then we don't want to touch it
            if (endOfLineCount == 0)
            {
                return triviaLine;
            }

            if (endOfLineCount > 1)
            {
                throw new ArgumentException($"Expected at most 1 end of line, received {endOfLineCount}");
            }

            if (!triviaLine.Last().IsKind(SyntaxKind.EndOfLineTrivia))
            {
                throw new ArgumentException("Expected the last trivia to be the end of line trivia");
            }

            // Comments make things a bit more complicated
            var comment = triviaLine.Any(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia))
                ? triviaLine.SingleOrDefault(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia))
                : (SyntaxTrivia?) null;

            triviaLine = triviaLine
                .Where((t, i) => comment == null || hasLeadingCode
                    ? !t.IsKind(SyntaxKind.WhitespaceTrivia)
                    : (i <= triviaLine.IndexOf(comment.Value) || !t.IsKind(SyntaxKind.WhitespaceTrivia))
                )
                .Select(t => !t.IsKind(SyntaxKind.SingleLineCommentTrivia) ? t : TriviaHelpers.NormaliseSingleLineComment(t))
                .Select(t => !t.IsKind(SyntaxKind.WhitespaceTrivia) || comment == null ? t : SyntaxFactory.Whitespace(new string(' ', indentWidth)))
                .ToSyntaxTriviaList();

            if (comment != null && hasLeadingCode)
            {
                // Ensure there is a single space between the end of the code and the start of the comment
                triviaLine = triviaLine.Prepend(SyntaxFactory.Whitespace(" ")).ToSyntaxTriviaList();
            }

            return triviaLine;
        }

        private static IEnumerable<SyntaxTriviaList> SplitByLine(SyntaxTriviaList triviaList)
        {
            var result = new List<SyntaxTriviaList>();
            var currentLine = new List<SyntaxTrivia>();

            foreach (var trivia in triviaList)
            {
                if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    currentLine.Add(trivia);
                    result.Add(currentLine.ToSyntaxTriviaList());
                    currentLine.Clear();
                }
                else
                {
                    currentLine.Add(trivia);
                }
            }

            if (currentLine.Any())
            {
                result.Add(currentLine.ToSyntaxTriviaList());
            }

            return result;
        }
    }
}
