using System.Xml.Serialization;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Xml.Views
{
    /// <summary>
    /// Represents a node <Views></Views> from XML Application Model.
    /// </summary>
    public class Views : XmlNode
    {

        /// <summary>
        /// Represent elements <ListView /> from XML Application Model.
        /// </summary>
        [XmlElement("ListView")]
        public ListView[] ListViews { get; set; }

        /// <summary>
        /// Represent elements <DetailView /> from XML Application Model.
        /// </summary>
        [XmlElement("DetailView")]
        public DetailView[] DetailViews { get; set; }

        /// <summary>
        /// Represent elements <LookupView /> from XML Application Model.
        /// </summary>
        [XmlElement("LookupView")]
        public LookupView[] LookupViews { get; set; }

    }
}