using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Statements
{
    public static class ExpressionStatementParser
    {
        public static Operation Parse(ExpressionStatementSyntax expressionStatement)
        {
            return BaseParser.Parse(expressionStatement.Expression) + Text(";");
        }
    }
}
