using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Languages
{
    public class LanguagesArray : XmlNode
    {

        [XmlElement("Language")]
        public Language[] Languages { get; set; }

    }
}