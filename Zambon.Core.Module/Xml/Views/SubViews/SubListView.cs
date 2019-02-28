using System.Xml.Serialization;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public class SubListView : BaseSubView
    {

        [XmlAttribute("ScrollSize")]
        public string ScrollSize { get; set; }

        public ListView ListView { get { return View as ListView; } }

    }
}