using System.Xml.Serialization;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Represents a node <SubListView /> from XML Application Model.
    /// </summary>
    public class SubListView : BaseSubView
    {

        /// <summary>
        /// The ScrollSize attribute from XML. Defines the size of the SubListView (sm - small, md - medium, lg - large).
        /// </summary>
        [XmlAttribute("ScrollSize")]
        public string ScrollSize { get; set; }

        /// <summary>
        /// The ListView view object.
        /// </summary>
        public ListView ListView { get { return View as ListView; } }

    }
}