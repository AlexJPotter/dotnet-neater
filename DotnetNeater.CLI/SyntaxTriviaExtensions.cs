using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace DotnetNeater.CLI
{
    public static class SyntaxTriviaExtensions
    {
        public static SyntaxTriviaList ToSyntaxTriviaList(this IEnumerable<SyntaxTrivia> syntaxTrivia) =>
            new SyntaxTriviaList(syntaxTrivia);
    }
}
