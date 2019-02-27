using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    public class ModuleConfiguration : XmlNode
    {

        [XmlElement("LoginDefaults")]
        public LoginDefaults LoginDefaults { get; set; }

        [XmlElement("DetailViewDefaults")]
        public DetailViewDefaults DetailViewDefaults { get; set; }

        [XmlElement("ListViewDefaults")]
        public ListViewDefaults ListViewDefaults { get; set; }

    }
}