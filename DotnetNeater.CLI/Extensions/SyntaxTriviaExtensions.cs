using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI.Extensions
{
    public static class SyntaxTriviaExtensions
    {
        public static SyntaxTriviaList ToSyntaxTriviaList(this IEnumerable<SyntaxTrivia> syntaxTrivia) =>
            new SyntaxTriviaList(syntaxTrivia);

        public static bool IsSingleLineComment(this SyntaxTrivia token) =>
            token.IsKind(SyntaxKind.SingleLineCommentTrivia);

        public static bool IsMultiLineComment(this SyntaxTrivia token) =>
            token.IsKind(SyntaxKind.MultiLineCommentTrivia);

        public static bool IsAnyComment(this SyntaxTrivia token) =>
            token.IsSingleLineComment() || token.IsMultiLineComment();

        public static bool IsEndOfLine(this SyntaxTrivia token) =>
            token.IsKind(SyntaxKind.EndOfLineTrivia);

        public static bool IsEndOfFile(this SyntaxTrivia token) =>
            token.IsKind(SyntaxKind.EndOfFileToken);
    }
}
