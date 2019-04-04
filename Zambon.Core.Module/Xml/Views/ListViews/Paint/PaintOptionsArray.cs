using System.Linq;
using System.Xml.Serialization;
using Zambon.Core.Module.Services;

namespace Zambon.Core.Module.Xml.Views.ListViews.Paint
{
    /// <summary>
    /// Represents a node <PaintOptions></PaintOptions> from XML Application Model.
    /// </summary>
    public class PaintOptionsArray : XmlNode
    {
        /// <summary>
        /// Represent elements <PaintOption /> from XML Application Model.
        /// </summary>
        [XmlElement("PaintOption")]
        public PaintOption[] PaintOption { get; set; }
    }
}