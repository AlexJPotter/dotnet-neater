using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Other
{
    public static class BlockParser
    {
        public static Operation Parse(BlockSyntax block)
        {
            return
                Text("{") +
                Nest(
                    4,
                    Line() +
                    Join(Nil(), block.Statements.Select(SyntaxTreeParser.Parse).ToList())
                ) +
                Line() +
                Text("}");
        }
    }
}
