namespace DotnetNeater.CLI.Operations
{
    public class GroupOperation : Operation
    {
        public Operation Operand { get; }

        public GroupOperation(Operation operand)
        {
            Operand = operand;
        }

        public override string Representation() =>
            $"group ({Operand.Representation()})";
    }
}
