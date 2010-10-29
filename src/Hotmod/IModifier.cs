using System.Xml.Linq;

namespace Hotmod
{
    public interface IModifier
    {
        XDocument Modify(XDocument document);
    }
}
