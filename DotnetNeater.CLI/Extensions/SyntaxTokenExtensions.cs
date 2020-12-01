using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI.Extensions
{
    public static class SyntaxTokenExtensions
    {
        public static bool IsSingleLineComment(this SyntaxToken token) =>
            token.IsKind(SyntaxKind.SingleLineCommentTrivia);

        public static bool IsMultiLineComment(this SyntaxToken token) =>
            token.IsKind(SyntaxKind.MultiLineCommentTrivia);

        public static bool IsEndOfLine(this SyntaxToken token) =>
            token.IsKind(SyntaxKind.EndOfLineTrivia);

        public static bool IsEndOfFile(this SyntaxToken token) =>
            token.IsKind(SyntaxKind.EndOfFileToken);

        public static void AssertIsKind(this SyntaxToken token, SyntaxKind kind)
        {
            if (!token.IsKind(kind))
            {
                throw new Exception($"Expected token to be of kind {kind} but was {token.Kind()}");
            }
        }
    }
}
