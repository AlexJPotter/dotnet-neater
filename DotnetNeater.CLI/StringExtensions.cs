namespace DotnetNeater.CLI
{
    public static class StringExtensions
    {
        public static string WithVisibleWhitespace(this string value) => value.Replace(" ", "·");
    }
}
