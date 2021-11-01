using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Names
{
    public static class IdentifierNameParser
    {
        public static Operation Parse(IdentifierNameSyntax identifierName)
        {
            return Operator.Text(identifierName.Identifier.Text.Trim());
        }
    }
}
