using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    public class Properties : XmlNode
    {

        [XmlElement("Property")]
        public Property[] Property { get; set; }

    }
}