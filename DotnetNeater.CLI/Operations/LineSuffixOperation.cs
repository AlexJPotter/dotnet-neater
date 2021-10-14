namespace DotnetNeater.CLI.Operations
{
    public class LineSuffixOperation : Operation
    {
        public Operation Operand { get; }

        public LineSuffixOperation(Operation operand)
        {
            Operand = operand;
        }

        public override string Representation() =>
            $"line-suffix ({Operand.Representation()})";
    }
}
