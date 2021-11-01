using DotnetNeater.CLI.Extensions;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Types
{
    public static class PredefinedTypeParser
    {
        public static Operation Parse(PredefinedTypeSyntax predefinedType)
        {
            return Operator.Text(predefinedType.Keyword.Text.WithoutSpaces());
        }
    }
}
