using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    public class EntityTypesArray : XmlNode
    {

        [XmlElement("Entity")]
        public Entity[] Entities { get; set; }

    }
}