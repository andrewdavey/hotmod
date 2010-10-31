using System.Web;
using System.Xml.Linq;
using Moq;
using Xunit;

namespace Hotmod.Modifiers
{
    public class AppRelativeUrlResolverTests
    {
        [Fact]
        public void a_href_url_resolved()
        {
            var document = XDocument.Parse("<html><a href=\"~/hello\">test</a></html>");
            var modifier = new AppRelativeUrlResolver();
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(r => r.ApplicationPath).Returns("/root");
            modifier.HttpRequest = httpRequest.Object;

            var output = modifier.Modify(document);

            var href = output.Root.Element("a").Attribute("href").Value;
            Assert.Equal("/root/hello", href);
        }

        [Fact]
        public void link_href_url_resolved()
        {
            var document = XDocument.Parse("<html><link href=\"~/hello\"/></html>");
            var modifier = new AppRelativeUrlResolver();
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(r => r.ApplicationPath).Returns("/root");
            modifier.HttpRequest = httpRequest.Object;

            var output = modifier.Modify(document);

            var href = output.Root.Element("link").Attribute("href").Value;
            Assert.Equal("/root/hello", href);
        }

        [Fact]
        public void img_src_url_resolved()
        {
            var document = XDocument.Parse("<html><img src=\"~/hello\"/></html>");
            var modifier = new AppRelativeUrlResolver();
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(r => r.ApplicationPath).Returns("/root");
            modifier.HttpRequest = httpRequest.Object;

            var output = modifier.Modify(document);

            var href = output.Root.Element("img").Attribute("src").Value;
            Assert.Equal("/root/hello", href);
        }


        [Fact]
        public void script_src_url_resolved()
        {
            var document = XDocument.Parse("<html><script src=\"~/hello\"></script></html>");
            var modifier = new AppRelativeUrlResolver();
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(r => r.ApplicationPath).Returns("/root");
            modifier.HttpRequest = httpRequest.Object;

            var output = modifier.Modify(document);

            var href = output.Root.Element("script").Attribute("src").Value;
            Assert.Equal("/root/hello", href);
        }
    }
}
