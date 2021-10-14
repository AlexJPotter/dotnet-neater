using System;

namespace DotnetNeater.CLI.Exceptions
{
    public class CodeDiscoveryException : Exception
    {
        public CodeDiscoveryException(string message) : base(message) { }
    }
}
