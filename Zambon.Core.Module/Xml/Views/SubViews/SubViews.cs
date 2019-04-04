using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Represents a node <SubViews></SubViews> from XML Application Model.
    /// </summary>
    public class SubViews : XmlNode
    {
        /// <summary>
        /// Internal property used to set the current level of modal views.
        /// </summary>
        [XmlIgnore]
        public int CurrentLevel { get; set; }

        /// <summary>
        /// Represent elements <DetailView /> from XML Application Model.
        /// </summary>
        [XmlElement("DetailView")]
        public DetailModal[] DetailViews { get; set; }

        /// <summary>
        /// Represent elements <LookupView /> from XML Application Model.
        /// </summary>
        [XmlElement("LookupView")]
        public LookupModal[] LookupViews { get; set; }

        /// <summary>
        /// Represent elements <SubListView /> from XML Application Model.
        /// </summary>
        [XmlElement("SubListView")]
        public SubListView[] SubListViews { get; set; }
    }
}