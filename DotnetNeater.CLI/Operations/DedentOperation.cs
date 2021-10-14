namespace DotnetNeater.CLI.Operations
{
    public class DedentOperation : Operation
    {
        public int Width { get; }

        public DedentOperation(int width)
        {
            Width = width;
        }

        public override string Representation() =>
            $"dedent {Width}";
    }
}
