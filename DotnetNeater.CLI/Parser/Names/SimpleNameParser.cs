using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Names
{
    public static class SimpleNameParser
    {
        public static Operation Parse(SimpleNameSyntax simpleName)
        {
            return Operator.Text(simpleName.Identifier.Text.Trim());
        }
    }
}
