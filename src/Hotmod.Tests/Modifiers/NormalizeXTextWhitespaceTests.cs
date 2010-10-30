using System.Xml.Linq;
using Xunit;

namespace Hotmod.Modifiers
{
    public class NormalizeXTextWhitespaceTests
    {
        [Fact]
        public void whitespace_in_p_tag_is_normalized()
        {
            var modifier = new NormalizeXTextWhitespace();
            var actual = modifier.Modify(new XDocument(new XElement("html", new XElement("p", "hello  world\t\t test"))));
            Assert.Equal(actual.Root.Element("p").Value, "hello world test");
        }

        [Fact]
        public void whitespace_in_pre_tags_remains_unchanged()
        {
            var modifier = new NormalizeXTextWhitespace();
            var actual = modifier.Modify(new XDocument(new XElement("html", new XElement("pre", "hello  world\t\t test"))));
            Assert.Equal(actual.Root.Element("pre").Value, "hello  world\t\t test");
        }
    }
}
