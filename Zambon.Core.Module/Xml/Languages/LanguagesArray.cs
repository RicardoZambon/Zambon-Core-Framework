using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Languages
{
    /// <summary>
    /// Represents a node <Languages></Languages> from XML Application Model.
    /// </summary>
    public class LanguagesArray : XmlNode
    {

        /// <summary>
        /// List of <Language /> elements.
        /// </summary>
        [XmlElement("Language")]
        public Language[] Languages { get; set; }

    }
}