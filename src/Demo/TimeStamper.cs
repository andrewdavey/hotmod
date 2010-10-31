using System;
using System.Xml.Linq;

namespace Demo
{
    /// <summary>
    /// Adds a comment to the end of the document containing the current time.
    /// </summary>
    public class TimeStamper : Hotmod.IModifier
    {
        public XDocument Modify(XDocument document)
        {
            document.Add(new XComment("Generated " + DateTime.UtcNow.ToString("r")));
            return document;
        }
    }
}