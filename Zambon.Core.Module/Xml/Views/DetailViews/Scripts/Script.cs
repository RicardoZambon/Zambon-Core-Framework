using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.DetailViews.Scripts
{
    /// <summary>
    /// Represents a node <Script /> from XML Application Model.
    /// </summary>
    public class Script : XmlNode
    {
        /// <summary>
        /// The Src attribute from XML. The source path to the .js script file.
        /// </summary>
        [XmlAttribute("Src")]
        public string Src { get; set; }
    }
}