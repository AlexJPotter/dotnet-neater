using System.Linq;
using DotnetNeater.CLI.Extensions;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Declarations
{
    public static class MethodDeclarationParser
    {
        public static Operation Parse(MethodDeclarationSyntax methodDeclaration)
        {
            var modifiersPart =
                Join(
                    Text(" "),
                    methodDeclaration.Modifiers.Select(token => Text(token.Text.WithoutSpaces())).ToList()
                );

            var returnTypePart = BaseParser.Parse(methodDeclaration.ReturnType);

            var methodName = Text(methodDeclaration.Identifier.Text.WithoutSpaces());

            var bodyPart = BaseParser.Parse(
                methodDeclaration.Body != null
                    ? methodDeclaration.Body
                    : methodDeclaration.ExpressionBody
            );

            return
                modifiersPart + Text(" ") + returnTypePart + Text(" ") + methodName + Text("()") +
                Line() +
                bodyPart;
        }
    }
}
