using System;

namespace DotnetNeater.CLI.Operations
{
    public class ConcatOperation : Operation
    {
        public Operation LeftOperand { get; }
        public Operation RightOperand { get; }

        public ConcatOperation(Operation leftOperand, Operation rightOperand)
        {
            if (leftOperand is ConcatOperation leftConcat)
            {
                throw new ArgumentException(
                    $"Should not instantiate ConcatOperation with another ConcatOperation as LeftOperand.\n" +
                    $"LeftOperand: {leftOperand.Representation()}\n" +
                    $"RightOperand: {rightOperand.Representation()}"
                );
            }

            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public override string Representation() =>
            $"{LeftOperand.Representation()} <> ({RightOperand.Representation()})";
    }
}
