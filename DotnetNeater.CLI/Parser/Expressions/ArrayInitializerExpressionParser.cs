using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Expressions
{
    public static class ArrayInitializerExpressionParser
    {
        public static Operation Parse(InitializerExpressionSyntax arrayInitializerExpression)
        {
            return
                Group(
                    Text("{") +
                        Nest(
                            4, // TODO - Make tab width configurable
                            arrayInitializerExpression.Expressions.Aggregate(
                                Nil(),
                                (current, next) =>
                                    current + Line() +
                                    BaseParser.Parse(next) + Text(",")
                            )
                        ) +
                        Line() +
                    Text("}")
                );
        }
    }
}
