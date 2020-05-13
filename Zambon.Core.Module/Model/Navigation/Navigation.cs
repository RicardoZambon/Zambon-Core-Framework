using System.Xml.Serialization;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Navigation
{
    /// <summary>
    /// Represents a node <Navigation></Navigation> from XML Application Model.
    /// </summary>
    public class Navigation : BaseNode
    {
        #region XML Elements

        /// <summary>
        /// Represents elements <Menu></Menu> from XML Application Model.
        /// </summary>
        [XmlElement("Menu")]
        public ChildItemCollection<Menu> Menus { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Navigation()
        {
            Menus = new ChildItemCollection<Menu>(this);
        }

        #endregion
    }
}