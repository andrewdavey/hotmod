using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Hotmod.Modifiers
{
    /// <summary>
    /// Replaces multiple whitespace characters inside all text nodes with a single space.
    /// However, whitespace inside &lt;pre&gt; elements is not changed.
    /// </summary>
    public class NormalizeTextWhitespace : IModifier
    {
        readonly Regex newlineAtEnd = new Regex(@"(\r\n|\r|\n)$", RegexOptions.Singleline);
        readonly Regex whitespace = new Regex(@"\s{2,}", RegexOptions.Singleline);

        public XDocument Modify(XDocument document)
        {
            var textNodes = document.DescendantNodes()
                .OfType<XText>()
                .Where(ShouldNormalize);

            foreach (var textNode in textNodes)
            {
                textNode.Value = NormalizeWhitespace(textNode.Value);
            }

            return document;
        }

        bool ShouldNormalize(XText textNode)
        {
            var tag = textNode.Parent.Name.LocalName;
            return !tag.Equals("pre", StringComparison.OrdinalIgnoreCase)
                && !tag.Equals("script", StringComparison.OrdinalIgnoreCase);
        }

        string NormalizeWhitespace(string text)
        {
            return whitespace.Replace(newlineAtEnd.Replace(text, " "), " ");
        }
    }
}
