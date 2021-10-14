namespace DotnetNeater.CLI.Printer
{
    public class PrinterState
    {
        public PrinterCommand CurrentCommand { get; private set; }

        public int CurrentIndentWidth { get; private set; } = 0;
        public int CurrentPositionOnLine { get; private set; } = 0;

        public bool ShouldRemeasure { get; private set; } = false; // What does this do?

        public void LoadNextCommand(PrinterInput input)
        {
            CurrentCommand = input.Read();
        }

        public void IncrementCurrentPositionOnLineBy(int width)
        {
            CurrentPositionOnLine += width;
        }

        public void ResetCurrentPositionOnLine()
        {
            CurrentPositionOnLine = 0;
        }

        public void SetShouldRemeasure(bool newValue)
        {
            ShouldRemeasure = newValue;
        }

        public void IncreaseCurrentIndentWidthBy(int increase)
        {
            CurrentIndentWidth += increase;
        }

        public void DecreaseCurrentIndentWidthBy(int decrease)
        {
            CurrentIndentWidth -= decrease;
        }
    }
}
