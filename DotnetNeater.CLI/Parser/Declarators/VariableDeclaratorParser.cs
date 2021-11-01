using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Declarators
{
    public static class VariableDeclaratorParser
    {
        public static Operation Parse(VariableDeclaratorSyntax variableDeclarator)
        {
            var variableName = variableDeclarator.Identifier.ValueText;

            var valueRepresentation =
                variableDeclarator.Initializer == null
                    ? null
                    : SyntaxTreeParser.Parse(variableDeclarator.Initializer.Value);

            return Text(variableName) + (valueRepresentation == null ? Nil() : Text(" = ") + valueRepresentation);
        }
    }
}
