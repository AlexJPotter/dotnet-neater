using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Other
{
    public static class ArgumentListParser
    {
        public static Operation Parse(ArgumentListSyntax argumentList)
        {
            return Group(
                Text("(") +
                Nest(
                    4,
                    SoftLine() + Join(Text(",") + Line(), argumentList.Arguments.Select(BaseParser.Parse).ToList())
                ) +
                SoftLine() +
                Text(")")
            );
        }
    }
}
