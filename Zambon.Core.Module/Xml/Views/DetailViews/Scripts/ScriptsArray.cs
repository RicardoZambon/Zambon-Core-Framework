using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.DetailViews.Scripts
{
    /// <summary>
    /// Represents a node <Scripts></Scripts> from XML Application Model.
    /// </summary>
    public class ScriptsArray : XmlNode
    {

        /// <summary>
        /// Represent elements <Script /> from XML Application Model.
        /// </summary>
        [XmlElement("Script")]
        public Script[] Script { get; set; }

    }
}