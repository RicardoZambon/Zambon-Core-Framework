using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    public class EntityTypes : XmlNode
    {

        [XmlElement("Entity")]
        public Entity[] Entities { get; set; }

    }
}
