using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Other
{
    public static class CompilationUnitParser
    {
        public static Operation Parse(CompilationUnitSyntax compilationUnit)
        {
            return
                compilationUnit.ChildNodes()
                    .Select(n => SyntaxTreeParser.Parse((CSharpSyntaxNode)n))
                    .Aggregate(Operator.Nil(), (current, next) => current + next);
        }
    }
}
