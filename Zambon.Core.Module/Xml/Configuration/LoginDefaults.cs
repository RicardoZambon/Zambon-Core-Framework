using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    public class LoginDefaults : XmlNode
    {

        [XmlAttribute("Theme")]
        public string Theme { get; set; }

    }
}