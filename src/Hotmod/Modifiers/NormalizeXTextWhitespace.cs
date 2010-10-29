using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

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
                textNode.Value = ModifyWhitespace(textNode.Value);
            }

            return document;
        }

        static bool NotInPreTag(XText textNode)
        {
            return !textNode.Parent.Name.LocalName.Equals("pre", StringComparison.OrdinalIgnoreCase);
        }
        
        string ModifyWhitespace(string text)
        {
            return whitespace.Replace(newlineAtEnd.Replace(text, " "), " ");
        }
    }
}
