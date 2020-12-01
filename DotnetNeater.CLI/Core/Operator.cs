using System;

namespace DotnetNeater.CLI.Core
{
    public static class Operator
    {
        public static Operation Concat(Operation left, Operation right)
        {
            return (left, right) switch
            {
                // (x <|> y) <> z = (x <> z) <|> (y <> z)
                (UnionOperation leftUnion, _) =>
                    Union(leftUnion.LeftOperand + right, leftUnion.RightOperand + right),

                // x <> (y <|> z) = (x <> y) <|> (y <|> z)
                (_, UnionOperation rightUnion) =>
                    Union(left + rightUnion.LeftOperand, left + rightUnion.RightOperand),

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

            // nest i (x <|> y) = nest i x <|> nest i y
            if (operand is UnionOperation unionOperand)
                return Union(Nest(indent, unionOperand.LeftOperand), Nest(indent, unionOperand.RightOperand));

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

                // flatten(x <|> y) = flatten(x)
                UnionOperation unionOperand =>
                    Flatten(unionOperand.LeftOperand),

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

            return Union(Flatten(operand), operand);
        }

        public static Operation Union(Operation leftOperand, Operation rightOperand)
        {
            return new UnionOperation(leftOperand, rightOperand);
        }
    }
}
