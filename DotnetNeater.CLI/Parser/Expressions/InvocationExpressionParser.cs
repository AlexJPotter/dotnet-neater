using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Expressions
{
    public static class InvocationExpressionParser
    {
        public static Operation Parse(InvocationExpressionSyntax invocationExpression)
        {
            return BaseParser.Parse(invocationExpression.Expression) +
                   BaseParser.Parse(invocationExpression.ArgumentList);
        }
    }
}
