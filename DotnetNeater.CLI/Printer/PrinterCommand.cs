using DotnetNeater.CLI.Core;

namespace DotnetNeater.CLI.Printer
{
    public class PrinterCommand
    {
        // TODO - Can we move BreakMode away to make it clearer how it's used?
        public BreakMode BreakMode { get; } // Only really relevant if it's a Group or a Line operation?
        public Operation Operation { get; }

        public PrinterCommand(BreakMode breakMode, Operation operation)
        {
            BreakMode = breakMode;
            Operation = operation;
        }
    }
}
