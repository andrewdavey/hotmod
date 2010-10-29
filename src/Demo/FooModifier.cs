using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Demo
{
    public class FooModifier : Hotmod.IModifier
    {

        public System.Xml.Linq.XDocument Modify(System.Xml.Linq.XDocument document)
        {
            document.Root.Element("body").Add(new XElement("p", "fooo!"));
            return document;
        }

    }
}