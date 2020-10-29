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
            var trailingTrivia = token.TrailingTrivia;

            // If the trailing trivia doesn't run to the end of the line then we don't want to touch it
            if (!trailingTrivia.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia)))
            {
                return token;
            }

            var includesComment = trailingTrivia.Any(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia));

            trailingTrivia = new SyntaxTriviaList(
                trailingTrivia
                    .Where(t => !t.IsKind(SyntaxKind.WhitespaceTrivia))
                    .Select(t =>
                        !t.IsKind(SyntaxKind.SingleLineCommentTrivia) ? t : TriviaHelpers.NormaliseSingleLineComment(t))
                    .ToList()
            );

            if (includesComment)
            {
                // Ensure there is a single space between the end of the line and the start of the comment
                trailingTrivia = new SyntaxTriviaList(trailingTrivia.Prepend(SyntaxFactory.Whitespace(" ")));
            }

            token = token.WithTrailingTrivia(trailingTrivia);

            return token;
        }
    }
}
