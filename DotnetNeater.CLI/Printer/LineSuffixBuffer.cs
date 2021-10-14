using System.Collections.Generic;
using System.Linq;

namespace DotnetNeater.CLI.Printer
{
    // Does this belong on the input?
    public class LineSuffixBuffer
    {
        private readonly List<PrinterCommand> _buffer = new();

        public bool Any()
        {
            return _buffer.Any();
        }

        public void FlushToInput(PrinterInput input)
        {
            // TODO - better encapsulate the stack (or move this into the input class?)
            _buffer.Reverse(); // The input uses a stack, so we have to reverse the buffer before writing it

            foreach (var command in _buffer)
            {
                input.Append(command.BreakMode, command.Operation);
            }

            _buffer.Clear();
        }

        public void Add(PrinterCommand command)
        {
            _buffer.Add(command);
        }
    }
}
