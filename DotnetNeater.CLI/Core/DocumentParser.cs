using System;

namespace DotnetNeater.CLI.Core
{
    public static class DocumentParser
    {
        public static IDocument ParseOperation(Operation operation)
        {
            if (operation is ConcatOperation concatOperation)
            {
                var leftOperand = concatOperation.LeftOperand;
                var rightOperand = concatOperation.RightOperand;

                return leftOperand switch
                {
                    NilOperation _ =>
                        ParseOperation(rightOperand),

                    TextOperation leftText =>
                        new TextDocument(leftText.Operand, ParseOperation(rightOperand)),

                    LineOperation _ =>
                        new LineDocument(0, ParseOperation(rightOperand)),

                    NestOperation nestLeft1 when nestLeft1.Operand is LineOperation =>
                        new LineDocument(nestLeft1.Indent, ParseOperation(rightOperand)),

                    NestOperation nestLeft2 when nestLeft2.Operand is ConcatOperation nestedConcatOperation && nestedConcatOperation.LeftOperand is LineOperation =>
                        new LineDocument(nestLeft2.Indent, ParseOperation(nestedConcatOperation.RightOperand)),

                    _ =>
                        throw new NotImplementedException($"ParseOperation({operation.Representation()}) : leftOperand = {leftOperand} ; rightOperand = {rightOperand}"),
                };
            }

            if (operation is NilOperation _)
            {
                return new NilDocument();
            }

            if (operation is TextOperation textOperation)
            {
                return new TextDocument(textOperation.Operand, new NilDocument());
            }

            if (operation is LineOperation _)
            {
                return new LineDocument(0, new NilDocument());
            }

            if (operation is NestOperation nestOperation)
            {
                if (nestOperation.Operand is LineOperation)
                {
                    return new LineDocument(nestOperation.Indent, new NilDocument());
                }

                if (nestOperation.Operand is ConcatOperation nestedConcatOperation && nestedConcatOperation.LeftOperand is LineOperation)
                {
                    return new LineDocument(nestOperation.Indent, ParseOperation(nestedConcatOperation.RightOperand));
                }
            }

            throw new NotImplementedException($"ParseOperation({operation.Representation()})");
        }
    }
}
