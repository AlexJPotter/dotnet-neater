using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Other
{
    public static class ArgumentParser
    {
        public static Operation Parse(ArgumentSyntax argument)
        {
            var nameColon = argument.NameColon;

            if (nameColon != null)
            {
                return BaseParser.Parse(nameColon.Name) + Text(": ") + BaseParser.Parse(argument.Expression);
            }

            return BaseParser.Parse(argument.Expression);
        }
    }
}
