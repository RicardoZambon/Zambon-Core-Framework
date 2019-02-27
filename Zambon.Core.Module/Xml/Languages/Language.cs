using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Languages
{
    public class Language : XmlNode
    {

        [XmlAttribute("Code"), MergeKey]
        public string Code { get; set; }

        [XmlAttribute("FlagIcon")]
        public string FlagIcon { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

    }
}