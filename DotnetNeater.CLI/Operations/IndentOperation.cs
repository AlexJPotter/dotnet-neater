namespace DotnetNeater.CLI.Operations
{
    public class IndentOperation : Operation
    {
        public int Width { get; }

        public IndentOperation(int width)
        {
            Width = width;
        }

        public override string Representation() =>
            $"indent {Width}";
    }
}
