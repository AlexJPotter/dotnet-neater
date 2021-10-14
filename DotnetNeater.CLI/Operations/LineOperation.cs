namespace DotnetNeater.CLI.Operations
{
    public class LineOperation : Operation
    {
        public bool IsSoft { get; }
        public bool IsHard { get; }
        public bool IsLiteral { get; }

        public LineOperation(bool isSoft = false, bool isHard = false, bool isLiteral = false)
        {
            IsSoft = isSoft;
            IsHard = isHard;
            IsLiteral = isLiteral;
        }

        public override string Representation() =>
            IsSoft ? "soft-line" : IsHard ? "hard-line" : IsLiteral ? "literal-line" : "line";

        //  If the line is "soft" it means it doesn't actually logically divide any code, otherwise it does so we should
        // introduce a space character. For example `users.Select(user => user.Name)` could have a soft line between
        // `users` and `.Select(user => user.Name)`, but `var name = user.Name` could not have a soft line between `var`
        // and `name` because the space matters.
        // TODO - Come up with a better name for stuff (soft/hard/literal)
        public string GetFlattenedString() =>
            IsSoft ? "" : " ";
    }
}
