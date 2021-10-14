using System.Collections.Generic;
using System.Linq;
using DotnetNeater.CLI.Operations;

namespace DotnetNeater.CLI.Printer
{
    public class PrinterInput
    {
        private readonly Stack<PrinterCommand> _commands = new();

        private PrinterInput() { }

        public static PrinterInput FromRootOperation(Operation rootOperation)
        {
            var printInput = new PrinterInput();
            printInput._commands.Push(new PrinterCommand(BreakMode.Break, rootOperation));
            return printInput;
        }

        public bool CanRead()
        {
            return _commands.Any();
        }

        public PrinterCommand Read()
        {
            return _commands.Pop();
        }

        public void Append(BreakMode breakMode, Operation operation)
        {
            _commands.Push(new PrinterCommand(breakMode, operation));
        }

        public IReadOnlyList<PrinterCommand> GetCommandList()
        {
            return _commands.ToList().AsReadOnly();
        }
    }
}
