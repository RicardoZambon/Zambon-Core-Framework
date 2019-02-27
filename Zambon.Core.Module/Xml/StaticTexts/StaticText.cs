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