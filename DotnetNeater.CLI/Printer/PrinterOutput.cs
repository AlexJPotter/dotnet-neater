namespace DotnetNeater.CLI.Printer
{
    public class PrinterOutput
    {
        private string _output = "";

        public string GetOutput()
        {
            return _output;
        }

        public void Append(string text)
        {
            _output += text;
        }
    }
}
