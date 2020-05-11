using System.Xml.Serialization;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module.Entities.Properties
{
    /// <summary>
    /// Represents a list of properties in XML model file.
    /// </summary>
    public class Properties : BaseNode
    {
        #region XML Elements

        /// <summary>
        /// List of all properties available in XML file.
        /// </summary>
        [XmlElement(nameof(Property))]
        public ChildItemCollection<Property> PropertiesList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Properties()
        {
            PropertiesList = new ChildItemCollection<Property>(this);
        }

        #endregion
    }    
}