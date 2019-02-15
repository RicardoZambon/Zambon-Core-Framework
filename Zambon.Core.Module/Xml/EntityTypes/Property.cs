using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    public class Property : XmlNode
    {

        [XmlAttribute("Name"), MergeKey]
        public string Name { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

    }
}