using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.StaticTexts
{
    public class StaticTexts : XmlNode
    {

        [XmlElement("StaticText")]
        public StaticText[] Texts { get; set; }

    }
}