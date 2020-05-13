using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.StaticTexts
{
    /// <summary>
    /// Represents a node <StaticText /> from XML Application Model.
    /// </summary>
    public class StaticText : BaseNode
    {
        #region XML Attributes

        /// <summary>
        /// The Key attribute from XML. The static text key should be used when displaying across the application.
        /// </summary>
        [XmlAttribute, Merge]
        public string Key { get; set; }

        /// <summary>
        /// The Value attribute from XML. The static text value.
        /// </summary>
        [XmlAttribute]
        public string Value { get; set; }

        #endregion
    }
}