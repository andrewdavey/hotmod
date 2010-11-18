using System.Xml.Linq;
using Xunit;

namespace Hotmod.Modifiers
{
    public class NormalizeTextWhitespaceTests
    {
        [Fact]
        public void whitespace_in_p_tag_is_normalized()
        {
            var modifier = new NormalizeTextWhitespace();
            var actual = modifier.Modify(new XDocument(new XElement("html", new XElement("p", "hello  world\t\t test"))));
            Assert.Equal(actual.Root.Element("p").Value, "hello world test");
        }

        [Fact]
        public void whitespace_in_pre_tags_remains_unchanged()
        {
            var modifier = new NormalizeTextWhitespace();
            var actual = modifier.Modify(new XDocument(new XElement("html", new XElement("pre", "hello  world\t\t test"))));
            Assert.Equal(actual.Root.Element("pre").Value, "hello  world\t\t test");
        }

        [Fact]
        public void whitespace_in_script_tags_remains_unchanged()
        {
            var modifier = new NormalizeTextWhitespace();
            var actual = modifier.Modify(new XDocument(new XElement("html", new XElement("script", "hello  world\t\t test"))));
            Assert.Equal(actual.Root.Element("script").Value, "hello  world\t\t test");
        }
    }
}
