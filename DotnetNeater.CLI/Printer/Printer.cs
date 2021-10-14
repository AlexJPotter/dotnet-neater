using System;
using System.Collections.Generic;
using DotnetNeater.CLI.Core;

namespace DotnetNeater.CLI.Printer
{
    public class Printer
    {
        private readonly int _preferredLineLength;

        private PrinterInput _input;
        private PrinterOutput _output = new();

        private PrinterCursor _cursor = new();

        private LineSuffixBuffer _lineSuffixBuffer = new();

        private PrinterCommand _currentCommand = null;
        private Operation CurrentOperation => _currentCommand.Operation;
        private BreakMode CurrentBreakMode => _currentCommand.BreakMode;

        private bool _shouldRemeasure = false;

        private Printer(int preferredLineLength)
        {
            _preferredLineLength = preferredLineLength;
        }

        public static Printer WithPreferredLineLength(int preferredLineLength)
        {
            return new Printer(preferredLineLength);
        }

        private void Initialise(PrinterInput input)
        {
            _input = input;
            _output = new();

            _cursor = new();

            _lineSuffixBuffer = new();

            _currentCommand = null;

            _shouldRemeasure = false;
        }

        public string Print(Operation rootOperation)
        {
            Initialise(PrinterInput.FromRootOperation(rootOperation));

            while (_input.CanRead())
            {
                LoadNextCommand();

                switch (CurrentOperation)
                {
                    case ConcatOperation concatOperation:
                        HandleConcatOperation(concatOperation);
                        break;
                    case NilOperation:
                        // Do nothing
                        break;
                    case TextOperation textOperation:
                        HandleTextOperation(textOperation);
                        break;
                    case LineOperation lineOperation:
                        HandleLineOperation(lineOperation);
                        break;
                    case LineSuffixOperation lineSuffixOperation:
                        HandleLineSuffixOperation(lineSuffixOperation);
                        break;
                    case LineSuffixBoundaryOperation lineSuffixBoundaryOperation:
                        HandleLineSuffixBoundaryOperation(lineSuffixBoundaryOperation);
                        break;
                    case NestOperation nestOperation:
                        HandleNestOperation(nestOperation);
                        break;
                    case IndentOperation indentOperation:
                        HandleIndentOperation(indentOperation);
                        break;
                    case DedentOperation dedentOperation:
                        HandleDedentOperation(dedentOperation);
                        break;
                    case GroupOperation groupOperation:
                        HandleGroupOperation(groupOperation);
                        break;
                    case BreakParentOperation _:
                        // I don't think we need this, I think a "hard line" has the right effect anyway
                        break;
                    default:
                        throw new ArgumentException($"Unrecognised operation: {CurrentOperation.Representation()}");
                }

                // If we've finished processing but still have some line suffix content in the buffer we need to flush
                // and process it before finishing.
                if (!_input.CanRead() && _lineSuffixBuffer.Any())
                {
                    _lineSuffixBuffer.FlushToInput(_input);
                }
            }

            return _output.GetOutput();
        }

        private void HandleConcatOperation(ConcatOperation concatOperation)
        {
            // TODO really should abstract away the stack
            AppendToInput(concatOperation.RightOperand); // Right operand first cause it's a stack...
            AppendToInput(concatOperation.LeftOperand);
        }

        private void HandleTextOperation(TextOperation textOperation)
        {
            var text = textOperation.Operand;
            WriteTextToOutput(text);
        }

        private void HandleLineOperation(LineOperation lineOperation)
        {
            if (CurrentBreakMode == BreakMode.Flat && !lineOperation.IsHard)
            {
                var textToPush = lineOperation.GetFlattenedString();
                WriteTextToOutput(textToPush);
            }
            else // We're in BreakMode.Break, or BreakMode.Flat but with a Hard Line
            {
                if (CurrentBreakMode == BreakMode.Flat) // The Line must be a Hard Line
                {
                    _shouldRemeasure = true;
                }

                if (_lineSuffixBuffer.Any())
                {
                    // Write the line first
                    // and then the line suffix content
                    // because the input is a stack, which is last-in first-out
                    // and we want to print all the line suffix content THEN the new line
                    AppendToInput(lineOperation);
                    _lineSuffixBuffer.FlushToInput(_input);
                }
                else
                {
                    WriteNewLineToOutput(lineOperation);
                }
            }
        }

        private void HandleLineSuffixOperation(LineSuffixOperation lineSuffixOperation)
        {
            _lineSuffixBuffer.Add(new PrinterCommand(CurrentBreakMode, lineSuffixOperation.Operand));
        }

        private void HandleLineSuffixBoundaryOperation(LineSuffixBoundaryOperation _)
        {
            if (!_lineSuffixBuffer.Any()) return;
            AppendToInput(new LineOperation(isHard: true));
        }

        private void HandleNestOperation(NestOperation nestOperation)
        {
            // again, order is what it is because of the stack...
            AppendToInput(new DedentOperation(nestOperation.IndentWidth));
            AppendToInput(nestOperation.Operand);
            AppendToInput(new IndentOperation(nestOperation.IndentWidth));
        }

        private void HandleIndentOperation(IndentOperation indentOperation)
        {
            _cursor.IncreaseCurrentIndentWidthBy(indentOperation.Width);
        }

        private void HandleDedentOperation(DedentOperation dedentOperation)
        {
            _cursor.DecreaseCurrentIndentWidthBy(dedentOperation.Width);
        }

        private void HandleGroupOperation(GroupOperation groupOperation)
        {
            if (CurrentBreakMode == BreakMode.Flat && !_shouldRemeasure)
            {
                AppendToInput(groupOperation.Operand);
            }
            else // (CurrentBreakMode == BreakMode.Hard || state.ShouldRemeasure)
            {
                _shouldRemeasure = false;

                var flatCommand = new PrinterCommand(
                    BreakMode.Flat, // We try to lay it out flat
                    groupOperation.Operand
                );

                AppendToInput(
                    operation: groupOperation.Operand,
                    overrideBreakMode: Fits(flatCommand) ? BreakMode.Flat : BreakMode.Break
                );
            }
        }

        private bool Fits(PrinterCommand next)
        {
            // How much room do we have left on the current line?
            var remainingSpaceOnLine = _preferredLineLength - _cursor.CurrentPositionOnLine;

            // All the stuff that is still to be placed (not necessarily on the same line)
            // Can we just duplicate the stack?
            var remainingCommandsList = _input.GetCommandList();

            // If we duplicate the stack we can safely just pop stuff
            var remainingCommandIndex = remainingCommandsList.Count; // Surely this is past the end of the list?

            // All the commands we're trying to fit into the remaining space (?)
            // If we duplicate the stack can we just get rid of this?
            var commands = new Stack<PrinterCommand>();
            commands.Push(next);

            // Everything we've placed in the remaining space
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

                if (operation is TextOperation textOperation)
                {
                    // Obviously we just append the text and consume some space on the line
                    output.Add(textOperation.Operand);
                    remainingSpaceOnLine -= textOperation.Operand.Length;
                }
                else if (operation is ConcatOperation concatOperation)
                {
                    // If we hit a ConcatOperation we have to "expand" it
                    commands.Push(new PrinterCommand(mode, concatOperation.RightOperand));
                    commands.Push(new PrinterCommand(mode, concatOperation.LeftOperand));
                }
                else if (operation is GroupOperation groupOperation)
                {
                    // If we hit a GroupOperation we have to "expand" it
                    commands.Push(new PrinterCommand(mode, groupOperation.Operand));
                }
                else if (operation is LineOperation lineOperation)
                {
                    if (mode == BreakMode.Break || lineOperation.IsHard)
                    {
                        // We've still got remaining space on the current line, and we're now ending it with a newline
                        return true;
                    }

                    // Otherwise, mode == BreakMode.Flat, so we flatten the line
                    var textToPush = lineOperation.GetFlattenedString();
                    output.Add(textToPush);
                    remainingSpaceOnLine -= textToPush.Length;
                }
            }

            return false;
        }

        private void WriteTextToOutput(string text)
        {
            _output.Append(text);
            _cursor.IncrementCurrentPositionOnLineBy(text.Length);
        }

        private void WriteNewLineToOutput(LineOperation lineOperation)
        {
            _output.Append(NewLine);
            _cursor.ResetCurrentPositionOnLine();

            if (!lineOperation.IsLiteral)
            {
                // TODO should literal lines still indent relative to somewhere?
                WriteIndentToOutput();
            }
        }

        private void WriteIndentToOutput()
        {
            WriteTextToOutput(new string(' ', _cursor.CurrentIndentWidth));
        }

        private void AppendToInput(Operation operation, BreakMode? overrideBreakMode = null)
        {
            _input.Append(overrideBreakMode ?? CurrentBreakMode, operation);
        }

        private void LoadNextCommand()
        {
            _currentCommand = _input.Read();
        }

        // TODO - Make this configurable somehow?
        private string NewLine => "\n";
    }
}
