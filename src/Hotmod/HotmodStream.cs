using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Hotmod.Configuration;

namespace Hotmod
{
    public class HotmodStream : MemoryStream
    {
        readonly Stream outputStream;
        readonly Encoding encoding;
        readonly HttpContextBase httpContext;
        readonly IEnumerable<IModifier> modifiers;
        readonly HotmodSection config;
        bool closing;

        public HotmodStream(Stream outputStream, HttpContextBase httpContext, HotmodSection config)
        {
            this.outputStream = outputStream;
            this.encoding = httpContext.Response.ContentEncoding;
            this.httpContext = httpContext;
            this.config = config;
            this.modifiers = CreateModifiers(config.Modifiers).ToArray();
        }
        
        public override void Close()
        {
            if (closing) return; // Using a StreamReader to read this will cause Close to be called again!
            closing = true;

            if (httpContext.Response.ContentType == "text/html" && httpContext.Response.StatusCode != 304/*not modified*/)
            {
                WriteFormattedHtml();
            }
            else
            {
                Position = 0;
                CopyTo(outputStream);
            }
            base.Close();
        }

        void WriteFormattedHtml()
        {
            var html = GetRawHtml();
            XDocument document;
            if (!TryParseHtmlWithErrorHandling(html, out document)) return;
            document = ApplyModifiers(document);
            OutputXDocument(document);
        }

        string GetRawHtml()
        {
            Position = 0;
            using (var reader = new StreamReader(this))
            {
                return HtmlEntityHelper.ConvertHtmlEntitiesToNumbers(reader.ReadToEnd());
            }
        }

        bool TryParseHtmlWithErrorHandling(string html, out XDocument document)
        {
            Position = 0;
            try
            {
                document = XDocument.Parse(html);
                return true;
            }
            catch (XmlException ex)
            {
                switch (config.HtmlParseError)
                {
                    case HtmlParseErrorMode.OutputOriginal:
                        using (var writer = new StreamWriter(outputStream, encoding))
                        {
                            writer.Write(html);
                            writer.Flush();
                        }
                        break;

                    case HtmlParseErrorMode.AppendException:
                        using (var writer = new StreamWriter(outputStream, encoding))
                        {
                            writer.WriteLine(html);
                            writer.WriteLine("<!--\r\n" + ex.ToString() + "\r\n-->");
                            writer.Flush();
                        }
                        break;

                    case HtmlParseErrorMode.Throw:
                    default:
                        throw;
                }
                document = null;
                return false;
            }
        }

        XDocument ApplyModifiers(XDocument document)
        {
            foreach (var modifier in modifiers)
            {
                document = modifier.Modify(document);
            }
            return document;
        }

        void OutputXDocument(XDocument document)
        {
            using (var output = new StreamWriter(outputStream, encoding))
            {
                WriteCorrectHtml5DocType(document, output);
                using (var xmlWriter = XmlWriter.Create(output, CreateXmlWriterSettings()))
                {
                    document.WriteTo(xmlWriter);
                }
            }
        }

        IEnumerable<IModifier> CreateModifiers(ModifierCollection modifierCollection)
        {
            return from c in modifierCollection
                   let type = Type.GetType(c.Type, true)
                   select (IModifier)Activator.CreateInstance(type);
        }

        /// <summary>
        /// The HTML 5 doctype is not written correctly by the XmlWriter, so handle writing it out manually.
        /// </summary>
        void WriteCorrectHtml5DocType(XDocument document, StreamWriter output)
        {
            if (HasHtml5DocType(document))
            {
                document.DocumentType.Remove();
                output.WriteLine("<!DOCTYPE html>");
            }
        }

        bool HasHtml5DocType(XDocument document)
        {
            return document.DocumentType != null &&
                   document.DocumentType.Name == "html" &&
                   string.IsNullOrEmpty(document.DocumentType.PublicId) &&
                   string.IsNullOrEmpty(document.DocumentType.SystemId);
        }

        XmlWriterSettings CreateXmlWriterSettings()
        {
            return new XmlWriterSettings
            {
                Encoding = encoding,
                Indent = IndentOutput,
                OmitXmlDeclaration = true
            };
        }

        bool IndentOutput
        {
            get
            {
                switch (config.PrettyPrint)
                {
                    case PrettyPrint.On:
                        return true;

                    case PrettyPrint.InDebugMode:
                        return httpContext.IsDebuggingEnabled;

                    case PrettyPrint.RemoveWhitespace:
                    default:
                        return false;
                }
            }
        }
    }


}
