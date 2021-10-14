namespace DotnetNeater.CLI.Printer
{
    public class PrinterCursor
    {
        public int CurrentIndentWidth { get; private set; } = 0;
        public int CurrentPositionOnLine { get; private set; } = 0;

        public void IncrementCurrentPositionOnLineBy(int width)
        {
            CurrentPositionOnLine += width;
        }

        public void ResetCurrentPositionOnLine()
        {
            CurrentPositionOnLine = 0;
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
