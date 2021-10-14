namespace DotnetNeater.CLI.Operations
{
    public abstract class Operation
    {
        public abstract string Representation();

        public static Operation operator +(Operation left, Operation right)
        {
            if (left is ConcatOperation leftConcat)
            {
                return leftConcat.LeftOperand + (leftConcat.RightOperand + right);
            }

            return Operator.Concat(left, right);
        }
    }
}
