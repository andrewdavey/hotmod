using System;
using System.Xml.Linq;

namespace Demo
{
    public class TimeStamper : Hotmod.IModifier
    {
        public XDocument Modify(XDocument document)
        {
            document.Add(new XComment("Generated " + DateTime.UtcNow.ToString("r")));
            return document;
        }
    }
}