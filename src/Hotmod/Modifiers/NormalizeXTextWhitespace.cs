using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Hotmod.Modifiers
{
    class NormalizeXTextWhitespace : IModifier
    {
        readonly Regex newlineAtEnd = new Regex(@"(\r\n|\r|\n)$", RegexOptions.Singleline);
        readonly Regex whitespace = new Regex(@"\s{2,}", RegexOptions.Singleline);

        public XDocument Modify(XDocument document)
        {
            var textNodes = document.DescendantNodes()
                .OfType<XText>()
                .Where(NotInPreTag);

            foreach (var textNode in textNodes)
            {
                textNode.Value = NormalizeWhitespace(textNode.Value);
            }

            return document;
        }

        static bool NotInPreTag(XText textNode)
        {
            return !textNode.Parent.Name.LocalName.Equals("pre", StringComparison.OrdinalIgnoreCase);
        }

        string NormalizeWhitespace(string text)
        {
            return whitespace.Replace(newlineAtEnd.Replace(text, " "), " ");
        }
    }
}
