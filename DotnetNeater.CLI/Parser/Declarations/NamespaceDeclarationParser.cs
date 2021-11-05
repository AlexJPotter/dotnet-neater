﻿using System.Linq;
using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Declarations
{
    public static class NamespaceDeclarationParser
    {
        public static Operation Parse(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            return
                Line() +
                Text("namespace ") + BaseParser.Parse(namespaceDeclaration.Name) +
                Line() +
                Text("{") +
                Nest(
                    4,
                    Line() +
                    Join(Nil(), namespaceDeclaration.Members.Select(BaseParser.Parse).ToList())
                )
                + Line()
                + Text("}")
                + Line();
        }
    }
}