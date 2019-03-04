using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    /// <summary>
    /// Represents a node <SearchProperties></SearchProperties> from XML Application Model.
    /// </summary>
    public class SearchPropertiesArray : XmlNode
    {

        /// <summary>
        /// Represent elements <SearchProperty /> from XML Application Model.
        /// </summary>
        [XmlElement("SearchProperty")]
        public SearchProperty[] SearchProperty { get; set; }

    }
}