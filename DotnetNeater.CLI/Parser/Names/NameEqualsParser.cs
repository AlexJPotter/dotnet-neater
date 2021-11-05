﻿using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Names
{
    public static class NameEqualsParser
    {
        public static Operation Parse(NameEqualsSyntax nameEquals)
        {
            return BaseParser.Parse(nameEquals.Name) + Operator.Text(" = ");
        }
    }
}
