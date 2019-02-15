using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views
{
    public class Views : XmlNode
    {

        [XmlElement("ListView")]
        public ListView[] ListViews { get; set; }

        [XmlElement("DetailView")]
        public DetailView[] DetailViews { get; set; }

        [XmlElement("LookupView")]
        public LookupView[] LookupViews { get; set; }

    }
}