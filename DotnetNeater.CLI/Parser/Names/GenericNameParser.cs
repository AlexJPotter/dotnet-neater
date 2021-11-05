using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Names
{
    public static class GenericNameParser
    {
        public static Operation Parse(GenericNameSyntax genericName)
        {
            return
                Operator.Text(genericName.Identifier.Text.Trim()) +
                BaseParser.Parse(genericName.TypeArgumentList);
        }
    }
}
