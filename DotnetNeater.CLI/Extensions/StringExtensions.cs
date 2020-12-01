namespace DotnetNeater.CLI.Extensions
{
    public static class StringExtensions
    {
        public static string WithVisibleWhitespace(this string value) => value.Replace(" ", "·");

        public static string WithoutSpaces(this string value) => value.Replace(" ", "");
    }
}
