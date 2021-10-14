namespace DotnetNeater.CLI.Operations
{
    public class TextOperation : Operation
    {
        public string Operand { get; }

        public TextOperation(string operand)
        {
            Operand = operand;
        }

        public override string Representation() =>
            $"text '{Operand}'";
    }
}
