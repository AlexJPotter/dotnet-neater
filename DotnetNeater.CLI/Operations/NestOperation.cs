namespace DotnetNeater.CLI.Operations
{
    public class NestOperation : Operation
    {
        public int IndentWidth { get; }
        public Operation Operand { get; }

        public NestOperation(int indentWidth, Operation operand)
        {
            IndentWidth = indentWidth;
            Operand = operand;
        }

        public override string Representation() =>
            $"nest {IndentWidth} ({Operand.Representation()})";
    }
}
