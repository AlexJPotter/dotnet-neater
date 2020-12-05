using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetNeater.CLI.Core
{
    public enum BreakMode
    {
        Break,
        Flat,
    }

    public class Command
    {
        public BreakMode BreakMode { get; }
        public Operation Operation { get; }

        public Command(BreakMode breakMode, Operation operation)
        {
            BreakMode = breakMode;
            Operation = operation;
        }
    }

    public static class PrintHelpers
    {
        public static string Print(int preferredLineLength, Operation rootOperation)
        {
            var commands = new Stack<Command>();
            var output = "";
            
            var currentIndent = 0;
            var currentPositionOnLine = 0;

            var shouldRemeasure = false;

            var lineSuffixBuffer = new List<Command>();

            commands.Push(new Command(BreakMode.Break, rootOperation));

            while (commands.Count > 0)
            {
                var command = commands.Pop();
                var operation = command.Operation;
                var mode = command.BreakMode;

                if (operation is ConcatOperation concatOperation)
                {
                    commands.Push(new Command(mode, concatOperation.RightOperand));
                    commands.Push(new Command(mode, concatOperation.LeftOperand));
                }
                else if (operation is NilOperation)
                {
                    // Do nothing
                }
                else if (operation is TextOperation textOperation)
                {
                    output += textOperation.Operand;
                    currentPositionOnLine += textOperation.Operand.Length;
                }
                else if (operation is LineOperation lineOperation)
                {
                    if (mode == BreakMode.Flat && !lineOperation.IsHard)
                    {
                        var textToPush = lineOperation.IsSoft ? "" : " ";
                        output += textToPush;
                        currentPositionOnLine += textToPush.Length;
                    }
                    else // We're in BreakMode.Break, or BreakMode.Flat but with a Hard Line
                    {
                        if (mode == BreakMode.Flat) // The Line must be a Hard Line
                        {
                            shouldRemeasure = true;
                        }

                        if (lineSuffixBuffer.Any())
                        {
                            commands.Push(new Command(mode, operation));
                            FlushLineSuffixBuffer();
                        }
                        else if (lineOperation.IsLiteral)
                        {
                            // TODO - Do we still need to indent relative to somewhere?
                            output += "\n"; // TODO - Normalised newlines
                            currentPositionOnLine = 0;
                        }
                        else
                        {
                            output += "\n"; // TODO - Normalised newlines
                            output += new string(' ', currentIndent);
                            currentPositionOnLine = currentIndent;
                        }
                    }
                }
                else if (operation is LineSuffixOperation lineSuffixOperation)
                {
                    lineSuffixBuffer.Add(new Command(mode, lineSuffixOperation.Operand));
                }
                else if (operation is LineSuffixBoundaryOperation)
                {
                    if (lineSuffixBuffer.Any())
                    {
                        commands.Push(new Command(mode, new LineOperation(isHard: true)));
                    }
                }
                else if (operation is NestOperation nestOperation)
                {
                    commands.Push(new Command(mode, new DedentOperation(nestOperation.Indent)));
                    commands.Push(new Command(mode, nestOperation.Operand));
                    commands.Push(new Command(mode, new IndentOperation(nestOperation.Indent)));
                }
                else if (operation is IndentOperation indentOperation)
                {
                    currentIndent += indentOperation.Size;
                }
                else if (operation is DedentOperation dedentOperation)
                {
                    currentIndent -= dedentOperation.Size;
                }
                else if (operation is GroupOperation groupOperation)
                {
                    if (mode == BreakMode.Flat && !shouldRemeasure)
                    {
                        commands.Push(new Command(BreakMode.Flat, groupOperation.Operand));
                    }
                    else
                    {
                        shouldRemeasure = false;

                        var next = new Command(BreakMode.Flat, groupOperation.Operand);
                        var remainingSpaceOnLine = preferredLineLength - currentPositionOnLine;

                        if (Fits(next, commands, remainingSpaceOnLine))
                        {
                            commands.Push(next);
                        }
                        else
                        {
                            commands.Push(new Command(BreakMode.Break, groupOperation.Operand));
                        }
                    }
                }

                if (!commands.Any() && lineSuffixBuffer.Any())
                {
                    FlushLineSuffixBuffer();
                }

                // Local function
                void FlushLineSuffixBuffer()
                {
                    lineSuffixBuffer.Reverse();

                    foreach (var command in lineSuffixBuffer)
                    {
                        commands.Push(command);
                    }

                    lineSuffixBuffer.Clear();
                }
            }

            return output;
        }

        private static bool Fits(Command next, Stack<Command> remainingCommands, int remainingSpaceOnLine)
        {
            var remainingCommandsList = remainingCommands.ToList();
            var remainingCommandIndex = remainingCommands.Count;

            var commands = new Stack<Command>();
            commands.Push(next);

            var output = new List<string>();

            while (remainingSpaceOnLine >= 0)
            {
                if (commands.Count == 0)
                {
                    if (remainingCommandIndex == 0)
                    {
                        return true;
                    }

                    commands.Push(remainingCommandsList[remainingCommandIndex - 1]);
                    remainingCommandIndex--;
                    continue;
                }

                var command = commands.Pop();
                var operation = command.Operation;
                var mode = command.BreakMode;

                if (operation is NilOperation)
                {
                    // Do nothing
                }
                else if (operation is TextOperation textOperation)
                {
                    output.Add(textOperation.Operand);
                    remainingSpaceOnLine -= textOperation.Operand.Length;
                }
                else if (operation is ConcatOperation concatOperation)
                {
                    commands.Push(new Command(mode, concatOperation.RightOperand));
                    commands.Push(new Command(mode, concatOperation.LeftOperand));
                }
                else if (operation is NestOperation) { }
                else if (operation is IndentOperation) { }
                else if (operation is DedentOperation) { }
                else if (operation is GroupOperation groupOperation)
                {
                    commands.Push(new Command(mode, groupOperation.Operand));
                }
                else if (operation is LineOperation lineOperation)
                {
                    if (mode == BreakMode.Break || lineOperation.IsHard)
                    {
                        return true;
                    }

                    var textToPush = lineOperation.IsSoft ? "" : " ";
                    output.Add(textToPush);
                    remainingSpaceOnLine -= textToPush.Length;
                }
            }

            return false;
        }

        public static string Pretty(int preferredLineLength, IDocument document)
        {
            return Best(preferredLineLength, 0, document).Layout();
        }

        private static ISimpleDocument Best(
            int availableWidth,
            int charactersAlreadyPlacedOnCurrentLine,
            IDocument document
        )
        {
            return document switch
            {
                // best w k Nil = Nil
                NilDocument nilDocument =>
                    nilDocument,

                // best w k (i ‘Line‘ x) = i ‘Line‘ best w i x
                LineDocument lineDocument =>
                    new LineDocument(lineDocument.Indent, Best(availableWidth, lineDocument.Indent, lineDocument.Document)),

                // best w k (s ‘Text‘ x) = s ‘Text‘ best w (k + length s) x
                TextDocument textDocument =>
                    new TextDocument(
                        textDocument.StringValue,
                        Best(availableWidth, charactersAlreadyPlacedOnCurrentLine + textDocument.StringValue.Length, textDocument.Document)
                    ),

                // best w k (x ‘Union‘ y) = better w k (best w k x) (best w k y)
                UnionDocument unionDocument =>
                    Better(
                        availableWidth,
                        charactersAlreadyPlacedOnCurrentLine,
                        Best(availableWidth, charactersAlreadyPlacedOnCurrentLine, unionDocument.LeftDocument),
                        Best(availableWidth, charactersAlreadyPlacedOnCurrentLine, unionDocument.RightDocument)
                    ),

                _ => throw new NotImplementedException($"Best({availableWidth}, {charactersAlreadyPlacedOnCurrentLine}, {document.Representation()})"),
            };
        }

        private static ISimpleDocument Better(
            int availableWidth,
            int charactersAlreadyPlacedOnCurrentLine,
            ISimpleDocument x,
            ISimpleDocument y
        )
        {
            if (Fits(availableWidth - charactersAlreadyPlacedOnCurrentLine, x))
            {
                return x;
            }

            return y;
        }

        private static bool Fits(int availableWidth, IDocument document)
        {
            if (availableWidth < 0) return false;

            return document switch
            {
                NilDocument _ =>
                    true,

                TextDocument textDocument =>
                    Fits(availableWidth - textDocument.StringValue.Length, textDocument.Document),

                LineDocument _ =>
                    true,

                _ => throw new NotImplementedException($"Fits({availableWidth}, {document.Representation()})"),
            };
        }
    }
}
