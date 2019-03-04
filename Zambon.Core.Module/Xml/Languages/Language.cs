using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Languages
{
    /// <summary>
    /// Represents a node <Language /> from XML Application Model.
    /// </summary>
    public class Language : XmlNode
    {

        /// <summary>
        /// The code of the language, the same from AppSettings.json file.
        /// </summary>
        [XmlAttribute("Code"), MergeKey]
        public string Code { get; set; }

        /// <summary>
        /// The icon should be used to display the language flag in languages list.
        /// </summary>
        [XmlAttribute("FlagIcon")]
        public string FlagIcon { get; set; }

        /// <summary>
        /// The language display name should be displayed in languages list.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

    }
}