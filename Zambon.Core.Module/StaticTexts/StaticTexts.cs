using System.Xml;
using System.Xml.Serialization;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module.StaticTexts
{
    /// <summary>
    /// Represents a node <StaticTexts></StaticTexts> from XML Application Model.
    /// </summary>
    public class StaticTexts : BaseNode
    {
        #region XML Elements

        /// <summary>
        /// Represent elements <StaticText /> from XML Application Model.
        /// </summary>
        [XmlElement(nameof(StaticText))]
        public ChildItemCollection<StaticText> TextsList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StaticTexts()
        {
            TextsList = new ChildItemCollection<StaticText>(this);
        }

        #endregion
    }
}