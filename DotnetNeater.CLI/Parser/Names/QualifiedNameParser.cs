using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetNeater.CLI.Parser.Names
{
    public static class QualifiedNameParser
    {
        public static Operation Parse(QualifiedNameSyntax qualifiedNameSyntax)
        {
            return
                SyntaxTreeParser.Parse(qualifiedNameSyntax.Left) +
                Operator.Text(".") +
                SyntaxTreeParser.Parse(qualifiedNameSyntax.Right);
        }
    }
}
