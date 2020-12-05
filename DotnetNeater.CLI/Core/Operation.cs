﻿using System;

namespace DotnetNeater.CLI.Core
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

    public class NilOperation : Operation
    {
        public override string Representation() =>
            "nil";
    }

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

    public class LineOperation : Operation
    {
        public bool IsSoft { get; }
        public bool IsHard { get; }
        public bool IsLiteral { get; }

        public LineOperation(bool isSoft = false, bool isHard = false, bool isLiteral = false)
        {
            IsSoft = isSoft;
            IsHard = isHard;
            IsLiteral = isLiteral;
        }

        public override string Representation() =>
            IsSoft ? "soft-line" : IsHard ? "hard-line" : IsLiteral ? "literal-line" : "line";
    }

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

    public class LineSuffixBoundaryOperation : Operation
    {
        public override string Representation() =>
            "line-suffix-boundary";
    }

    public class NestOperation : Operation
    {
        public int Indent { get; }
        public Operation Operand { get; }

        public NestOperation(int indent, Operation operand)
        {
            Indent = indent;
            Operand = operand;
        }

        public override string Representation() =>
            $"nest {Indent} ({Operand.Representation()})";
    }

    public class IndentOperation : Operation
    {
        public int Size { get; }
        
        public IndentOperation(int size)
        {
            Size = size;
        }

        public override string Representation() =>
            $"indent {Size}";
    }

    public class DedentOperation : Operation
    {
        public int Size { get; }

        public DedentOperation(int size)
        {
            Size = size;
        }

        public override string Representation() =>
            $"dedent {Size}";
    }

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

    public class BreakParentOperation : Operation
    {
        public override string Representation() =>
            "break-parent";
    }
}
