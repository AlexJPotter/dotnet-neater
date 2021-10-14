using System;

namespace DotnetNeater.CLI.Obsolete
{
    public interface IDocument
    {
        string Layout();
        string Representation();
    }

    public interface ISimpleDocument : IDocument { }

    public class NilDocument : ISimpleDocument
    {
        public string Layout() => "";

        public string Representation() => "Nil";
    }

    public class TextDocument : ISimpleDocument
    {
        public string StringValue { get; }
        public IDocument Document { get; }

        public TextDocument(string stringValue, IDocument document)
        {
            StringValue = stringValue;
            Document = document;
        }

        public string Layout() => StringValue + Document.Layout();

        public string Representation() => $"\"{StringValue}\" 'Text' ({Document.Representation()})";
    }

    public class LineDocument : ISimpleDocument
    {
        public int Indent { get; }
        public IDocument Document { get; }

        public LineDocument(int indent, IDocument document)
        {
            Indent = indent;
            Document = document;
        }

        public string Layout() => "\n" + new string(' ', Indent) + Document.Layout();

        public string Representation() => $"\n{Indent} 'Line' ({Document.Representation()})";
    }

    public class UnionDocument : IDocument
    {
        public IDocument LeftDocument { get; }
        public IDocument RightDocument { get; }

        public UnionDocument(IDocument leftDocument, IDocument rightDocument)
        {
            LeftDocument = leftDocument;
            RightDocument = rightDocument;
        }

        public string Layout() => throw new NotImplementedException($"Layout({Representation()})");

        public string Representation() => $"{LeftDocument.Representation()} 'Union' {RightDocument.Representation()}";
    }
}
