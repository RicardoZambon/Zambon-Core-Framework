using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.DetailViews.Scripts
{
    public class ScriptsArray : XmlNode
    {

        [XmlElement("Script")]
        public Script[] Script { get; set; }

    }
}