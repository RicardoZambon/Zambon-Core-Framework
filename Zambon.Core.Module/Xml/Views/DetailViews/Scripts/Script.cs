using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.DetailViews.Scripts
{
    public class Script : XmlNode
    {

        [XmlAttribute("Src")]
        public string Src { get; set; }

    }
}