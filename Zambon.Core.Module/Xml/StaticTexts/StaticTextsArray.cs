using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.StaticTexts
{
    public class StaticTextsArray : XmlNode
    {

        [XmlElement("StaticText")]
        public StaticText[] Texts { get; set; }

    }
}