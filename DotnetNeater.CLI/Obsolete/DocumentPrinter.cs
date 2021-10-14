using System;

namespace DotnetNeater.CLI.Obsolete
{
    public static class DocumentPrinter
    {
        public static string Pretty(int preferredLineLength, IDocument document)
        {
            return Best(preferredLineLength, 0, document).Layout();
        }

        private static ISimpleDocument Best(
            int availableWidth,
            int charactersAlreadyPlacedOnCurrentLine,
            IDocument document
        )
        {
            return document switch
            {
                // best w k Nil = Nil
                NilDocument nilDocument =>
                    nilDocument,

                // best w k (i ‘Line‘ x) = i ‘Line‘ best w i x
                LineDocument lineDocument =>
                    new LineDocument(lineDocument.Indent, Best(availableWidth, lineDocument.Indent, lineDocument.Document)),

                // best w k (s ‘Text‘ x) = s ‘Text‘ best w (k + length s) x
                TextDocument textDocument =>
                    new TextDocument(
                        textDocument.StringValue,
                        Best(availableWidth, charactersAlreadyPlacedOnCurrentLine + textDocument.StringValue.Length, textDocument.Document)
                    ),

                // best w k (x ‘Union‘ y) = better w k (best w k x) (best w k y)
                UnionDocument unionDocument =>
                    Better(
                        availableWidth,
                        charactersAlreadyPlacedOnCurrentLine,
                        Best(availableWidth, charactersAlreadyPlacedOnCurrentLine, unionDocument.LeftDocument),
                        Best(availableWidth, charactersAlreadyPlacedOnCurrentLine, unionDocument.RightDocument)
                    ),

                _ => throw new NotImplementedException($"Best({availableWidth}, {charactersAlreadyPlacedOnCurrentLine}, {document.Representation()})"),
            };
        }

        private static ISimpleDocument Better(
            int availableWidth,
            int charactersAlreadyPlacedOnCurrentLine,
            ISimpleDocument x,
            ISimpleDocument y
        )
        {
            if (Fits(availableWidth - charactersAlreadyPlacedOnCurrentLine, x))
            {
                return x;
            }

            return y;
        }

        private static bool Fits(int availableWidth, IDocument document)
        {
            if (availableWidth < 0) return false;

            return document switch
            {
                NilDocument _ =>
                    true,

                TextDocument textDocument =>
                    Fits(availableWidth - textDocument.StringValue.Length, textDocument.Document),

                LineDocument _ =>
                    true,

                _ => throw new NotImplementedException($"Fits({availableWidth}, {document.Representation()})"),
            };
        }
    }
}
