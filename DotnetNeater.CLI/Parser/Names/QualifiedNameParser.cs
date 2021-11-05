using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Names
{
    public static class QualifiedNameParser
    {
        public static Operation Parse(QualifiedNameSyntax qualifiedNameSyntax)
        {
            return
                BaseParser.Parse(qualifiedNameSyntax.Left) +
                Operator.Text(".") +
                BaseParser.Parse(qualifiedNameSyntax.Right);
        }
    }
}
