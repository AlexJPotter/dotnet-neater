using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Statements
{
    public static class GlobalStatementParser
    {
        public static Operation Parse(GlobalStatementSyntax globalStatement)
        {
            return
                globalStatement.ChildNodes()
                    .Select(n => SyntaxTreeParser.Parse((CSharpSyntaxNode)n))
                    .Aggregate(Operator.Nil(), (current, next) => current + next);
        }
    }
}
