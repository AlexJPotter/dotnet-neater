using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Expressions
{
    public static class StringLiteralExpressionParser
    {
        public static Operation Parse(LiteralExpressionSyntax stringLiteralExpression)
        {
            return Operator.Text(stringLiteralExpression.Token.Text.Trim());
        }
    }
}
