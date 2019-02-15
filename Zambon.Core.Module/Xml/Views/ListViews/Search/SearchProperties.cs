using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    public class SearchProperties : XmlNode
    {

        [XmlElement("SearchProperty")]
        public SearchProperty[] SearchProperty { get; set; }

    }
}