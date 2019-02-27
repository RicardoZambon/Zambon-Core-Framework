using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    public class SearchPropertiesArray : XmlNode
    {

        [XmlElement("SearchProperty")]
        public SearchProperty[] SearchProperty { get; set; }

    }
}