using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module.Entities.Properties
{
    /// <summary>
    /// Represents properties listed under Entity in XML model.
    /// </summary>
    public class Property : BaseNode
    {
        #region XML Attributes

        /// <summary>
        /// The property name.
        /// </summary>
        [XmlAttribute("Name"), Merge]
        public string Name { get; set; }

        /// <summary>
        /// The text when displaying this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The text when displaying the input for this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute("Prompt")]
        public string Prompt { get; set; }

        /// <summary>
        /// The text when displaying the description for this property, default value from the DisplayName attribute.
        /// </summary>
        [XmlAttribute("Description")]
        public string Description { get; set; }

        #endregion
    }
}