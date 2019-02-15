using Zambon.Core.Module.Xml.Views.ListViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public class SubListView : BaseSubView
    {

        [XmlAttribute("ScrollSize")]
        public string ScrollSize { get; set; }

        public ListView ListView { get { return View as ListView; } }

    }
}