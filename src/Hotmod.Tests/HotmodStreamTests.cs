using System.IO;
using System.Text;
using System.Web;
using Hotmod.Configuration;
using Moq;
using Xunit;
using System.Xml;

namespace Hotmod
{
    public class HotmodStreamTests
    {
        HotmodStream streamUnderTest;
        HotmodSection config;
        MemoryStream outputStream;
        Mock<HttpContextBase> httpContext;

        public HotmodStreamTests()
        {
            config = new HotmodSection();
            outputStream = new MemoryStream();
            httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Response.OutputStream).Returns(outputStream);
            httpContext.Setup(c => c.Response.ContentEncoding).Returns(Encoding.ASCII);
            streamUnderTest = new HotmodStream(outputStream, httpContext.Object, config);
        }

        protected void WriteHtml(string html)
        {
            using (var writer = new StreamWriter(streamUnderTest))
            {
                writer.Write(html);
            }
        }

        protected string GetOutputHtml()
        {
            return Encoding.ASCII.GetString(outputStream.ToArray());
        }

        protected void AssertOutputHtml(string expectedHtml)
        {
            Assert.Equal(expectedHtml, GetOutputHtml());
        }

        [Fact]
        public void prety_print_on_formats_output()
        {
            config.PrettyPrint = PrettyPrint.On;
            WriteHtml(@"<!DOCTYPE html>
<html>
<head><title>Test</title></head>
<body>
<h1>Test</h1>
</body>
</html>");
            AssertOutputHtml(@"<!DOCTYPE html>
<html>
  <head>
    <title>Test</title>
  </head>
  <body>
    <h1>Test</h1>
  </body>
</html>");
        }
        
        [Fact]
        public void pretty_print_off_removes_spaces()
        {
            config.PrettyPrint = PrettyPrint.RemoveWhitespace;
            WriteHtml(@"<!DOCTYPE html>
<html>
<head><title>Test</title></head>
<body>
<h1>Test</h1>
</body>
</html>");
            AssertOutputHtml(@"<!DOCTYPE html>
<html><head><title>Test</title></head><body><h1>Test</h1></body></html>");
        }

        [Fact]
        public void prety_print_InDebugMode_and_debug_true_formats_output()
        {
            config.PrettyPrint = PrettyPrint.InDebugMode;
            httpContext.Setup(c => c.IsDebuggingEnabled).Returns(true);
            WriteHtml(@"<!DOCTYPE html>
<html>
<head><title>Test</title></head>
<body>
<h1>Test</h1>
</body>
</html>");
            AssertOutputHtml(@"<!DOCTYPE html>
<html>
  <head>
    <title>Test</title>
  </head>
  <body>
    <h1>Test</h1>
  </body>
</html>");
        }

        [Fact]
        public void prety_print_InDebugMode_and_debug_false_formats_output()
        {
            config.PrettyPrint = PrettyPrint.InDebugMode;
            httpContext.Setup(c => c.IsDebuggingEnabled).Returns(false);
            WriteHtml(@"<!DOCTYPE html>
<html>
<head><title>Test</title></head>
<body>
<h1>Test</h1>
</body>
</html>");
            AssertOutputHtml(@"<!DOCTYPE html>
<html><head><title>Test</title></head><body><h1>Test</h1></body></html>");
        }

        [Fact]
        public void invalid_html_HtmlParseErrorMode_OutputOriginal()
        {
            config.HtmlParseError = HtmlParseErrorMode.OutputOriginal;
            WriteHtml("<not valid html!");
            AssertOutputHtml("<not valid html!");
        }

        [Fact]
        public void invalid_html_HtmlParseErrorMode_AppendException()
        {
            config.HtmlParseError = HtmlParseErrorMode.AppendException;
            WriteHtml("<not valid html!");
            Assert.True(GetOutputHtml().StartsWith("<not valid html!\r\n<!--\r\nSystem.Xml.XmlException:"));
        }

        [Fact]
        public void invalid_html_HtmlParseErrorMode_Throw()
        {
            config.HtmlParseError = HtmlParseErrorMode.Throw;
            Assert.Throws<XmlException>(delegate
            {
                WriteHtml("<not valid html!");
            });
        }
    }
}
