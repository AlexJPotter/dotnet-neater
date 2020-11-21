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
            // Leading trivia can include multiple lines preceding the token, some of which may have trailing whitespace
            token = token.WithLeadingTrivia(RemoveTrailingWhitespaceFromTrivia(token, token.LeadingTrivia));

            token = token.WithTrailingTrivia(RemoveTrailingWhitespaceFromTrivia(token, token.TrailingTrivia));

            return token;
        }

        private static SyntaxTriviaList RemoveTrailingWhitespaceFromTrivia(SyntaxToken token, SyntaxTriviaList triviaList)
        {
            var triviaGroupedByLine = GroupTriviaByLine(triviaList).ToList();

            return triviaGroupedByLine
                .SelectMany((triviaForSingleLine, lineIndex) =>
                    RemoveTrailingWhitespaceTriviaForSingleLine(token, triviaForSingleLine).ToList()
                )
                .ToSyntaxTriviaList();
        }

        private static SyntaxTriviaList RemoveTrailingWhitespaceTriviaForSingleLine(
            SyntaxToken token,
            SyntaxTriviaList triviaForSingleLine
        )
        {
            var endOfLineCount = triviaForSingleLine.Count(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            // If the trivia doesn't run to the end of the line (or the end of the file) then we don't want to touch it
            if (endOfLineCount == 0 && !token.IsKind(SyntaxKind.EndOfFileToken))
                return triviaForSingleLine;

            if (endOfLineCount > 1)
                throw new ArgumentException($"Expected at most 1 end of line, received {endOfLineCount}");

            if (!token.IsKind(SyntaxKind.EndOfFileToken) && !triviaForSingleLine.Last().IsKind(SyntaxKind.EndOfLineTrivia))
                throw new ArgumentException("Expected the last trivia to be the end of line trivia");

            // Comments make things a bit more complicated
            var comment = triviaForSingleLine.Any(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia))
                ? triviaForSingleLine.SingleOrDefault(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia))
                : (SyntaxTrivia?) null;

            var indexOfComment = comment == null ? (int?) null : triviaForSingleLine.IndexOf(comment.Value);

            triviaForSingleLine = triviaForSingleLine
                .Where((t, i) => indexOfComment == null
                    // If no comment, just remove all whitespace
                    ? !t.IsKind(SyntaxKind.WhitespaceTrivia)
                    // Otherwise only remove whitespace that comes after the comment
                    : (i <= indexOfComment.Value || !t.IsKind(SyntaxKind.WhitespaceTrivia))
                )
                .Select(t => !t.IsKind(SyntaxKind.SingleLineCommentTrivia)
                    ? t
                    // Trailing whitespace after a single line comment is treated as part of the comment text
                    : TriviaHelpers.TrimTrailingWhitespaceFromSingleLineComment(t)
                )
                .ToSyntaxTriviaList();

            return triviaForSingleLine;
        }

        private static IEnumerable<SyntaxTriviaList> GroupTriviaByLine(SyntaxTriviaList triviaList)
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
