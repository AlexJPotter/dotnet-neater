using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Expressions
{
    public static class SimpleMemberAccessExpressionParser
    {
        public static Operation Parse(MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            return BaseParser.Parse(memberAccessExpressionSyntax.Expression) +
                   Operator.Text(".") +
                   BaseParser.Parse(memberAccessExpressionSyntax.Name);
        }
    }
}
