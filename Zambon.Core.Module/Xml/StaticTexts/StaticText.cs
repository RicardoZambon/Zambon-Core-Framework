using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.StaticTexts
{
    public class StaticText : XmlNode
    {

        [XmlAttribute("Key"), MergeKey]
        public string Key { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

    }
}