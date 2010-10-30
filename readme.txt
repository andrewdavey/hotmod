Hotmod - HTML output transformation module.

Hotmod is an HTTP Module for ASP.NET. It captures the HTML output of aspx pages,
MVC views and any "text/html" content created by the web request pipeline.
The HTML is parsed into a System.Xml.Linq.XDocument and then processed by 
"modifiers" before being reformatted and sent to the client. The resulting HTML
can be "pretty printed" (nicely indented) or have all insignificant whitespace
removed.

Modifiers are implementations of the the Hotmod.IModifier interface. They have
a single Modify method that receives the XDocument and returns a modified
XDocument.
For example, you can create a modifier that appends a timestamp comment to the 
end of the page. Or, do more complex things like find all <a> href's and change
the URLs from app relative to absolute. Using XLinq makes it very easy to alter
the HTML elements and attributes.

To use hotmod:
1) Reference hotmod.dll from your asp.net web application.
2) Then update your web.config file to use hotmod.
   The following basic web.config file explains the different bits to add...

<?xml version="1.0"?>
<configuration>
  <configSections>
    <!-- Tell the config system about the "hotmod" section. -->
    <section name="hotmod" type="Hotmod.Configuration.HotmodSection, Hotmod"/>
  </configSections>
  
  <system.web>
    <httpModules>
      <!-- You must add the hotmod module here so it can hook into the request pipeline and modify your HTML output. -->
      <add name="hot" type="Hotmod.Module, Hotmod"/>
    </httpModules>
  </system.web>
  
  <system.webServer>
    <modules>
      <!-- Hook hotmod into the IIS 7 pipeline using this module -->
      <add name="hot" type="Hotmod.Module, Hotmod"/>
    </modules>
  </system.webServer>

  <!--
  Configure hotmod.
  Attributes:
    prettyPrint         Defines how to format the output.
      InDebugMode       (Default) Enables pretty print when then web application config is set to Debug mode. In Release mode RemoveWhitespace is used instead.
      On                Always pretty print HTML output.
      RemoveWhitespace  Remove all insignificant whitespace from HTML output.

    htmlParseError      Defines how to handle parse errors in HTML. e.g. Unclosed tags.
      Throw             (Default) Throw the XmlException that describes the problem.
      AppendException   Output the original HTML and append the exception ToString content in a comment.
      OutputOriginal    Output the original HTML with not modifications.

  <modifiers/>
  Each modifier defines a name and a type. "name" can be any string identifier unique in the collection.
  "type" must be the Type full name string of an implementation of the Hotmod.IModifier interface.
  -->
  <hotmod prettyPrint="InDebugMode" htmlParseError="AppendException">
    <modifiers>
      <!-- add any custom implementations of Hotmod.IModifier here -->
      <add name="foo" type="Demo.TimeStamper, Demo" />
    </modifiers>
  </hotmod>

</configuration>