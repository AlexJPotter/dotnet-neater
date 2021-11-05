using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Expressions
{
    public static class ImplicitArrayCreationExpressionParser
    {
        public static Operation Parse(ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpression)
        {
            return Group(Text("new[]") + Line() + BaseParser.Parse(implicitArrayCreationExpression.Initializer));
        }
    }
}
