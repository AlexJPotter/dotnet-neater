using Microsoft.CodeAnalysis.CSharp;

namespace DotnetNeater.CLI
{
    public interface IFormattingRule
    {
        public string Name { get; }
        public CSharpSyntaxNode ApplyToNode(CSharpSyntaxNode node);
    }
}
