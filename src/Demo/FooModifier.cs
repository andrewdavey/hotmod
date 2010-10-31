using System.Xml.Linq;

namespace Demo
{
    /// <summary>
    /// Adds a paragraph element to the end of the body.
    /// </summary>
    public class FooModifier : Hotmod.IModifier
    {
        public XDocument Modify(XDocument document)
        {
            document.Root.Element("body").Add(new XElement("p", "fooo!"));
            return document;
        }
    }
}