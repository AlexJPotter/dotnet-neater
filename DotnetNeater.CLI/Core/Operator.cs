using System;

namespace DotnetNeater.CLI.Core
{
    public static class Operator
    {
        public static Operation Concat(Operation left, Operation right)
        {
            return new ConcatOperation(left, right);
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

        // Used for trailing comments and stuff
        public static Operation LineSuffix(Operation operation)
        {
            return new LineSuffixOperation(operation);
        }

        // Used to create a hard break in a line suffix
        public static Operation LineSuffixBoundary()
        {
            return new LineSuffixBoundaryOperation();
        }

        /// <summary>
        /// The nest operation adds indentation to a document.
        /// </summary>
        /// <param name="indent">The number of spaces by which to indent.</param>
        /// <param name="operand"></param>
        /// <returns></returns>
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

        /// <summary>
        /// The flatten operator replaces each line break (and its associated indentation) by a single space. A document
        /// always represents a non-empty set of layouts, where all layouts in the set flatten to the same layout.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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

                LineOperation lineOperation =>
                    Text(lineOperation.IsSoft ? "" : " "),

                NestOperation nestOperand =>
                    Flatten(nestOperand.Operand),

                // flatten(group(x)) = flatten(flatten(x) <|> x) = flatten(flatten(x)) = flatten(x)
                GroupOperation groupOperand =>
                    Flatten(groupOperand.Operand),

                _ => throw new NotImplementedException($"Flatten({operand.Representation()})"),
            };
        }

        /// <summary>
        /// Given a document, representing a set of layouts, group returns the set with one new element added,
        /// representing the layout in which everything is compressed on one line. This is achieved by replacing each
        /// newline (and the corresponding indentation) with text consisting of a single space.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
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
