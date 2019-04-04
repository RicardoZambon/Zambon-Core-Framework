using System;
using System.Xml.Serialization;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Navigation
{
    /// <summary>
    /// Represents a node <Navigation></Navigation> from XML Application Model.
    /// </summary>
    public class NavigationArray : XmlNode
    {
        /// <summary>
        /// Represents elements <Menu></Menu> from XML Application Model.
        /// </summary>
        [XmlElement("Menu")]
        public Menu[] Menus { get; set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            if ((Menus?.Length ?? 0) > 0)
                Array.Sort(Menus);

            base.OnLoadingXml(app, ctx);
        }

        #endregion
    }
}