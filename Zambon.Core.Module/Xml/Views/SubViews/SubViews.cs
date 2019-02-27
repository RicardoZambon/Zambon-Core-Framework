using Zambon.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public class SubViews : XmlNode
    {
        [XmlIgnore]
        public int CurrentLevel { get; set; }
        
        [XmlElement("DetailView")]
        public DetailModal[] DetailViews { get; set; }

        [XmlElement("LookupView")]
        public LookupModal[] LookupViews { get; set; }

        [XmlElement("SubListView")]
        public SubListView[] SubListViews { get; set; }

    }
}