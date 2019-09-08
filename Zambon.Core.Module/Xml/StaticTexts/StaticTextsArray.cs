using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.StaticTexts
{
    /// <summary>
    /// Represents a node <StaticTexts></StaticTexts> from XML Application Model.
    /// </summary>
    public class StaticTextsArray : XmlNode
    {
        #region XML Elements

        /// <summary>
        /// Represent elements <StaticText /> from XML Application Model.
        /// </summary>
        [XmlElement("StaticText")]
        public StaticText[] Texts { get; set; }

        #endregion
    }
}