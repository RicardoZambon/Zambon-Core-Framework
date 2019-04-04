using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    /// <summary>
    /// Represents a node <Properties></Properties> from XML Application Model.
    /// </summary>
    public class Properties : XmlNode
    {
        /// <summary>
        /// Represent elements <Property /> from XML Application Model.
        /// </summary>
        [XmlElement("Property")]
        public Property[] Property { get; set; }
    }
}