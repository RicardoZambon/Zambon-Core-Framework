using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.DetailViews;

namespace Zambon.Core.Module.Xml.Navigation
{
    /// <summary>
    /// Represents a node <Menu></Menu> from XML Application Model.
    /// </summary>
    public class Menu : XmlNode, IComparable
    {

        /// <summary>
        /// The Id attribute from XML. The ID of the menu item.
        /// </summary>
        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        /// <summary>
        /// The DisplayName attribute from XML. The display name of the menu item, if null and of type DetailView or ListView will use the title from the view.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The Icon attribute from XML. The icon of the menu item, if null and of type DetailView or ListView will use the icon from the view.
        /// </summary>
        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// The Index attribute from XML. The index order of the menu item.
        /// </summary>
        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { int.TryParse(value, out int index); Index = index; }
        }
        /// <summary>
        /// The Index attribute from XML. The index order of the menu item.
        /// </summary>
        [XmlIgnore]
        public int? Index { get; set; }

        /// <summary>
        /// The Type attribute from XML. The type defines what the action the menu should execute when accessed.
        /// </summary>
        [XmlAttribute("Type")]
        public string Type { get; set; }

        /// <summary>
        /// The ViewID attribute from XML. The ViewID of the view the menu should show, when the type is DetailView or ListView.
        /// </summary>
        [XmlAttribute("ViewID")]
        public string ViewID { get; set; }

        /// <summary>
        /// The ControllerName attribute from XML. The controller of the action the menu should execute when accessed, only used with DetailView type, by default will get the same controller from view.
        /// </summary>
        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        /// <summary>
        /// The ActionName attribute from XML. The action the menu should execute when accessed, only used with DetailView type.
        /// </summary>
        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        /// <summary>
        /// The ReportPath attribute from XML. The path of the report the menu should open, only used with Report type.
        /// </summary>
        [XmlAttribute("ReportPath")]
        public string ReportPath { get; set; }

        /// <summary>
        /// The URL attribute from XML. The URL the menu should open, only used with External type.
        /// </summary>
        [XmlAttribute("URL")]
        public string URL { get; set; }

        /// <summary>
        /// The ShowBadge attribute from XML. Indicates if the menu should display the badge along with the name, only used with submenus or ListViews, indicating the number of records (count) are available.
        /// </summary>
        [XmlAttribute("ShowBadge"), Browsable(false)]
        public string BoolShowBadge
        {
            get { return ShowBadge?.ToString(); }
            set { bool.TryParse(value, out bool showBadge); ShowBadge = showBadge; }
        }
        /// <summary>
        /// The ShowBadge attribute from XML. Indicates if the menu should display the badge along with the name, only used with submenus or ListViews, indicating the number of records (count) are available.
        /// </summary>
        [XmlIgnore]
        public bool? ShowBadge { get; set; }

        /// <summary>
        /// The submenus included in this menu. If the menu has submenus the menu Type is ignored.
        /// </summary>
        [XmlElement("Menu")]
        public Menu[] SubMenus { get; set; }


        /// <summary>
        /// The view object from ViewID, when available.
        /// </summary>
        [XmlIgnore]
        public BaseView View { get; private set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            if (SubMenus != null && SubMenus.Length > 0)
                Array.Sort(SubMenus);

            switch (Type)
            {
                case "ListView":
                    View = app.FindListView(ViewID);
                    break;
                case "DetailView":
                    View = app.FindDetailView(ViewID);
                    if (string.IsNullOrWhiteSpace(ControllerName))
                        ControllerName = ((DetailView)View).ControllerName;
                    break;
            }

            if (View != null)
            {
                View.OnLoadingXml(app, ctx);

                if (string.IsNullOrWhiteSpace(DisplayName))
                    DisplayName = View.Title;

                if (string.IsNullOrWhiteSpace(Icon))
                    Icon = View.Icon;
            }

            base.OnLoadingXml(app, ctx);
        }

        internal override void OnLoadingUserModel(Application app, CoreDbContext ctx)
        {
            switch (Type)
            {
                case "ListView":
                    View = app.FindListView(ViewID);
                    break;
                case "DetailView":
                    View = app.FindDetailView(ViewID);
                    break;
            }

            base.OnLoadingUserModel(app, ctx);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the Index with other menu object, to sort the menus array.
        /// </summary>
        /// <param name="obj">The menu object to capare to.</param>
        /// <returns>A signed number indicating the relative values of this instance and value. Return
        //     Value Description Less than zero: This instance is less than value. Zero: This
        //     instance is equal to value. Greater than zero: This instance is greater than value.</returns>
        public int CompareTo(object obj)
        {
            if (obj is Menu objMenu)
                return (Index ?? 0).CompareTo(objMenu.Index ?? 0);
            throw new ArgumentException("Object is not a menu.");
        }

        /// <summary>
        /// Checks if the informed user has access to this menu item.
        /// </summary>
        /// <param name="user">The user to check the access.</param>
        /// <returns>If the user has access returns true.</returns>
        public bool UserHasAccess(IUsers user)
        {
            switch (Type)
            {
                case "ListView":
                    if (!string.IsNullOrEmpty(Id) && !user.UserHasAccessToMenuID(Id, Type))
                        return false;

                    var entity = View?.GetEntityTypeName();
                    return user.UserHasAccessToType(entity, 3 /*Navigate*/);
                default:
                    return !string.IsNullOrEmpty(Id) && user.UserHasAccessToMenuID(Id, Type);
            }
        }

        #endregion

    }
}