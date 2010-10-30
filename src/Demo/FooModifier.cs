using System.Xml.Linq;

namespace Demo
{
    public class FooModifier : Hotmod.IModifier
    {
        public XDocument Modify(XDocument document)
        {
            document.Root.Element("body").Add(new XElement("p", "fooo!"));
            return document;
        }
    }
}