using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.DetailViews.Scripts
{
    public class Script : XmlNode
    {

        [XmlAttribute("Src")]
        public string Src { get; set; }

    }
}