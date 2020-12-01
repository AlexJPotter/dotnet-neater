using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static void AssertIsKind(this CSharpSyntaxNode syntaxNode, SyntaxKind kind)
        {
            if (!syntaxNode.IsKind(kind))
            {
                throw new Exception($"Expected node to be of kind {kind} but was {syntaxNode?.Kind()}");
            }
        }
    }
}
