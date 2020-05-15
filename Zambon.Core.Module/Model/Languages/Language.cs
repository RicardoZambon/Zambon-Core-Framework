using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Languages
{
    /// <summary>
    /// Represents a node <Language /> from XML Application Model.
    /// </summary>
    public class Language : BaseNode
    {
        #region XML Attributes

        /// <summary>
        /// The code of the language, the same from AppSettings.json file.
        /// </summary>
        [XmlAttribute, MergeKey]
        public string Code { get; set; }

        /// <summary>
        /// The icon should be used to display the language flag in languages list.
        /// </summary>
        [XmlAttribute]
        public string FlagIcon { get; set; }

        /// <summary>
        /// The language display name should be displayed in languages list.
        /// </summary>
        [XmlAttribute]
        public string DisplayName { get; set; }

        #endregion
    }
}