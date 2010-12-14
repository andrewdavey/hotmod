using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Hotmod.Modifiers
{
    public class AppRelativeUrlResolver : IModifier
    {
        public XDocument Modify(XDocument document)
        {
            var elementsWithHref = document.Descendants()
                .Where(e => e.Name.LocalName == "a" || e.Name.LocalName == "link");
            var elementsWithSrc = document.Descendants()
                .Where(e => e.Name.LocalName == "img" || e.Name.LocalName == "script");
            var formElementsWithAction = document.Descendants()
                .Where(e => e.Name.LocalName == "form");
            var root = HttpRequest.ApplicationPath.TrimEnd('/');
            
            foreach (var element in elementsWithHref)
            {
                ExpandUrl(element, "href", root);
            }
            foreach (var element in elementsWithSrc)
            {
                ExpandUrl(element, "src", root);
            }
            foreach (var element in formElementsWithAction)
            {
                ExpandUrl(element, "action", root);
            }

            return document;
        }

        public HttpRequestBase HttpRequest
        {
            get
            {
                if (httpRequest == null) httpRequest = new HttpRequestWrapper(HttpContext.Current.Request);
                return httpRequest;
            }
            set { httpRequest = value; }
        }

        HttpRequestBase httpRequest;

        void ExpandUrl(XElement element, string attributeName, string root)
        {
            var href = element.Attribute(attributeName);
            if (href != null && href.Value.StartsWith("~"))
            {
                href.Value = root + href.Value.Substring(1);
            }
        }
    }
}
