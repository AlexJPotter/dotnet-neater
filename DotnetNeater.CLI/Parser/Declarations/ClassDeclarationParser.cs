using System.Linq;
using DotnetNeater.CLI.Extensions;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Declarations
{
    public static class ClassDeclarationParser
    {
        public static Operation Parse(ClassDeclarationSyntax classDeclaration)
        {
            var modifiersPart =
                Join(
                    Text(" "),
                    classDeclaration.Modifiers.Select(token => Text(token.Text.WithoutSpaces())).ToList()
                );

            return
                modifiersPart + Text(" class ") + Text(classDeclaration.Identifier.Text.WithoutSpaces()) +
                Line() +
                Text("{") +
                Nest(
                    4,
                    Line() +
                    Join(Nil(), classDeclaration.Members.Select(BaseParser.Parse).ToList())
                ) +
                Line() +
                Text("}");
        }
    }
}
