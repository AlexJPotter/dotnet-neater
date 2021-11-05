using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Declarations
{
    public static class VariableDeclarationParser
    {
        public static Operation Parse(VariableDeclarationSyntax variableDeclaration)
        {
            var typeSyntax = variableDeclaration.Type;
            var typeText = Text(typeSyntax.GetText().ToString().Trim()); // TODO - might need more complicated rules

            var variables = variableDeclaration.Variables.Select(BaseParser.Parse).ToList();

            return typeText + Text(" ") + Join(Text(", "), variables);
        }
    }
}
