using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Other
{
    public static class TypeArgumentListParser
    {
        public static Operation Parse(TypeArgumentListSyntax typeArgumentList)
        {
            var arguments = typeArgumentList.Arguments.Select(SyntaxTreeParser.Parse).ToList();
            return Text("<") + Join(Text(", "), arguments) + Text(">");
        }
    }
}
