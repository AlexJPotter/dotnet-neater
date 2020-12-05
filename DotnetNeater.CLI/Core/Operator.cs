using System;

namespace DotnetNeater.CLI.Core
{
    public static class Operator
    {
        public static Operation Concat(Operation left, Operation right)
        {
            return (left, right) switch
            {
                _ =>
                    new ConcatOperation(left, right)
            };
        }

        public static Operation Nil()
        {
            return new TextOperation("");
        }

        public static Operation Text(string value)
        {
            if (value.Contains("\n"))
            {
                throw new Exception("Value passed to Text() should not contain newlines");
            }

            return new TextOperation(value);
        }

        public static Operation Line()
        {
            return new LineOperation();
        }

        public static Operation SoftLine()
        {
            return new LineOperation(isSoft: true);
        }

        public static Operation HardLine()
        {
            return new LineOperation(isHard: true) + new BreakParentOperation();
        }

        public static Operation LiteralLine()
        {
            return new LineOperation(isHard: true, isLiteral: true) + new BreakParentOperation();
        }

        public static Operation LineSuffix(Operation operation)
        {
            return new LineSuffixOperation(operation);
        }

        public static Operation LineSuffixBoundary()
        {
            return new LineSuffixBoundaryOperation();
        }

        public static Operation Nest(int indent, Operation operand)
        {
            if (indent == 0)
                return operand;

            if (operand is NestOperation nestOperand)
                return Nest(indent + nestOperand.Indent, nestOperand.Operand);

            if (operand is NilOperation nilOperand)
                return nilOperand;

            if (operand is TextOperation textOperand)
                return textOperand;

            if (operand is ConcatOperation concatOperand)
                return Concat(Nest(indent, concatOperand.LeftOperand), Nest(indent, concatOperand.RightOperand));

            return new NestOperation(indent, operand);
        }

        public static Operation Flatten(Operation operand)
        {
            return operand switch
            {
                ConcatOperation concatOperand =>
                    Flatten(concatOperand.LeftOperand) + Flatten(concatOperand.RightOperand),

                NilOperation nilOperand =>
                    nilOperand,

                TextOperation textOperand =>
                    textOperand,

                LineOperation _ =>
                    Text(" "), // TODO - Determine what this should be for different types of line break

                NestOperation nestOperand =>
                    Flatten(nestOperand.Operand),

                // flatten(group(x)) = flatten(flatten(x) <|> x) = flatten(flatten(x)) = flatten(x)
                GroupOperation groupOperand =>
                    Flatten(groupOperand.Operand),

                _ => throw new NotImplementedException($"Flatten({operand.Representation()})"),
            };
        }

        public static Operation Group(Operation operand)
        {
            // group(text(s) <> x) = text(s) <> group(x)
            if (operand is ConcatOperation concatOperand && concatOperand.LeftOperand is TextOperation nestedTextOperand)
            {
                return nestedTextOperand + Group(concatOperand.RightOperand);
            }

            if (operand is NilOperation nilOperand)
                return nilOperand;

            if (operand is TextOperation textOperand)
                return textOperand;

            return new GroupOperation(operand);
        }
    }
}
