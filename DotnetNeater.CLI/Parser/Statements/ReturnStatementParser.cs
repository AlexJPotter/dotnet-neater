using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Statements
{
    public static class ReturnStatementParser
    {
        public static Operation Parse(ReturnStatementSyntax returnStatement)
        {
            var shouldHaveLeadingNewLine =
                returnStatement.GetLeadingTrivia()
                    .Any(trivia => trivia.Kind() == SyntaxKind.EndOfLineTrivia);

            return (shouldHaveLeadingNewLine ? Line() : Nil()) + Group(
                Text("return") +
                Nest(
                    4,
                    Line() + SyntaxTreeParser.Parse(returnStatement.Expression)
                ) +
                Text(";")
            );
        }
    }
}
