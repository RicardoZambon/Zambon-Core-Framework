using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public class NavigationParentBase<TMenu> : SerializeNodeBase, INavigationParent<TMenu>
        where TMenu : MenuBase<TMenu>
    {
        #region Constants

        private const string MENUS_NODE = "Menu";

        #endregion

        #region XML Elements

        [XmlElement(MENUS_NODE)]
        public ChildItemCollection<TMenu> MenusList { get; set; }

        #endregion

        #region Constructors

        public NavigationParentBase()
        {
            MenusList = new ChildItemCollection<TMenu>(this);
        }

        #endregion
    }
}