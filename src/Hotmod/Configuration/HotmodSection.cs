using System.Configuration;

namespace Hotmod.Configuration
{
    public class HotmodSection : ConfigurationSection
    {
        [ConfigurationProperty("prettyPrint", DefaultValue = PrettyPrint.InDebugMode)]
        public PrettyPrint PrettyPrint
        {
            get { return (PrettyPrint)this["prettyPrint"]; }
            set { this["prettyPrint"] = value; }
        }

        [ConfigurationProperty("htmlParseError", DefaultValue = HtmlParseErrorMode.Throw)]
        public HtmlParseErrorMode HtmlParseError
        {
            get { return (HtmlParseErrorMode)this["htmlParseError"]; }
            set { this["htmlParseError"] = value; }
        }

        [ConfigurationCollection(typeof(ModifierElement))]
        [ConfigurationProperty("modifiers", IsDefaultCollection = true)]
        public ModifierCollection Modifiers
        {
            get { return (ModifierCollection)this["modifiers"]; }
        }
    }
}
