using System.Xml.Linq;

namespace Hotmod
{
    /// <summary>
    /// Modifies an <see cref="System.Xml.Linq.XDocument" />.
    /// </summary>
    public interface IModifier
    {
        /// <summary>
        /// Returns a modified document.
        /// </summary>
        /// <param name="document">The document to change.</param>
        /// <returns>The modified document. This may be the same instance passed in the document parameter or a new XDocument.</returns>
        XDocument Modify(XDocument document);
    }
}
