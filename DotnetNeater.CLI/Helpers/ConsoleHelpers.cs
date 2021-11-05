using System;

namespace DotnetNeater.CLI.Helpers
{
    public static class ConsoleHelpers
    {
        public static void WriteLine(ConsoleColor color, string text)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }

        public static void Warn(string text)
        {
            WriteLine(ConsoleColor.Yellow, text);
        }
    }
}
