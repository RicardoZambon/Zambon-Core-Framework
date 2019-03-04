using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    /// <summary>
    /// Represents a node <Property /> from XML Application Model.
    /// </summary>
    public class Property : XmlNode
    {

        /// <summary>
        /// The Name attribute from XML. The name of the property.
        /// </summary>
        [XmlAttribute("Name"), MergeKey]
        public string Name { get; set; }

        /// <summary>
        /// The DisplayName attribute from XML. The display name should be used in application, by default will use the DisplayName attribute.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

    }
}