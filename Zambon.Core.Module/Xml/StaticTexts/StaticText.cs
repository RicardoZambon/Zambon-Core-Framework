using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.StaticTexts
{
    /// <summary>
    /// Represents a node <StaticText /> from XML Application Model.
    /// </summary>
    public class StaticText : XmlNode
    {
        #region XML Attributes

        /// <summary>
        /// The Key attribute from XML. The static text key should be used when displaying across the application.
        /// </summary>
        [XmlAttribute("Key"), MergeKey]
        public string Key { get; set; }

        /// <summary>
        /// The Value attribute from XML. The static text value.
        /// </summary>
        [XmlAttribute("Value")]
        public string Value { get; set; }

        #endregion
    }
}