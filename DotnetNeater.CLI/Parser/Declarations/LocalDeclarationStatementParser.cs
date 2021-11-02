using DotnetNeater.CLI.Operations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser.Declarations
{
    public static class LocalDeclarationStatementParser
    {
        public static Operation Parse(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            return SyntaxTreeParser.Parse(localDeclarationStatement.Declaration) + Text(";") + Line();
        }
    }
}
