using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    public class DetailViewDefaults : XmlNode
    {

        [XmlAttribute("DefaultAction")]
        public string DefaultAction { get; set; }

        [XmlAttribute("DefaultView")]
        public string DefaultView { get; set; }        

    }
}