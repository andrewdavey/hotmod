using System.Collections.Generic;
using System.Configuration;
using Hotmod.Modifiers;

namespace Hotmod.Configuration
{
    public class ModifierCollection : ConfigurationElementCollection, IEnumerable<ModifierElement>
    {
        public ModifierCollection()
        {
            AddDefaultModifiers();
        }

        void AddDefaultModifiers()
        {
            BaseAdd(new ModifierElement
            {
                Name = "NormalizeTextWhitespace",
                Type = typeof(NormalizeTextWhitespace).AssemblyQualifiedName
            });
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModifierElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (ModifierElement)element;
        }

        IEnumerator<ModifierElement> IEnumerable<ModifierElement>.GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (ModifierElement)enumerator.Current;
            }
        }
    }
}
