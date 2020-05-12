using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Serialization;

namespace Zambon.Core.Module.Navigation
{
    /// <summary>
    /// Represents a node <Menu></Menu> from XML Application Model.
    /// </summary>
    public class Menu : BaseNode, IComparable//, IIcon
    {
        public const string MenuTypeDetailView = "DetailView";
        public const string MenuTypeListView = "ListView";
        public const string MenuTypeSeparator = "Separator";
        public const string MenuTypeReport = "Report";
        public const string MenuTypeExternalURL = "ExternalURL";

        #region XML Attributes 

        /// <summary>
        /// The Id attribute from XML. The ID of the menu item.
        /// </summary>
        [XmlAttribute, Merge]
        public string Id { get; set; }

        /// <summary>
        /// The DisplayName attribute from XML. The display name of the menu item, if null and of type DetailView or ListView will use the title from the view.
        /// </summary>
        [XmlAttribute]
        public string DisplayName { get; set; }

        /// <summary>
        /// The Icon attribute from XML. The icon of the menu item, if null and of type DetailView or ListView will use the icon from the view.
        /// </summary>
        [XmlAttribute]
        public string Icon { get; set; }

        /// <summary>
        /// The Index attribute from XML. The index order of the menu item.
        /// </summary>
        [XmlAttribute(nameof(Index)), Browsable(false)]
        public string IntIndex
        {
            get { return Index?.ToString(); }
            set { if (value != null) { int.TryParse(value, out int index); Index = index; } }
        }

        /// <summary>
        /// The Type attribute from XML. The type defines what the action the menu should execute when accessed.
        /// </summary>
        [XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// The ViewID attribute from XML. The ViewID of the view the menu should show, when the type is DetailView or ListView.
        /// </summary>
        [XmlAttribute]
        public string ViewID { get; set; }


        /// <summary>
        /// The ReportPath attribute from XML. The path of the report the menu should open, only used with Report type.
        /// </summary>
        [XmlAttribute]
        public string ReportPath { get; set; }


        /// <summary>
        /// The URL attribute from XML. The URL the menu should open, only used with External type.
        /// </summary>
        [XmlAttribute]
        public string URL { get; set; }


        /// <summary>
        /// The ShowBadge attribute from XML. Indicates if the menu should display the badge along with the name, only used with sub menus or ListViews, indicating the number of records (count) are available.
        /// </summary>
        [XmlAttribute, Browsable(false)]
        public string BoolShowBadge
        {
            get { return ShowBadge?.ToString(); }
            set { if (value != null) { bool.TryParse(value, out bool showBadge); ShowBadge = showBadge; } }
        }

        [XmlAttribute]
        public string BadgeQuery { get; set; }

        [XmlAttribute]
        public string BadgeQueryArguments { get; set; }

        #endregion

        #region XML Elements

        /// <summary>
        /// The sub menus included in this menu. If the menu has sub menus the menu Type is ignored.
        /// </summary>
        [XmlElement(nameof(Menu))]
        public ChildItemCollection<Menu> SubMenus { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The Index attribute from XML. The index order of the menu item.
        /// </summary>
        [XmlIgnore]
        public int? Index { get; set; }

        /// <summary>
        /// If the menu item or any sub menu has the attribute ShowBadge set to true will return true as well.
        /// </summary>
        //[XmlIgnore]
        //public bool HasBadge { get { return (SubMenus?.Length ?? 0) > 0 ? SubMenus.Any(x => x.HasBadge) : (ShowBadge ?? false); } }

        /// <summary>
        /// The ShowBadge attribute from XML. Indicates if the menu should display the badge along with the name, only used with sub menus or ListViews, indicating the number of records (count) are available.
        /// </summary>
        [XmlIgnore]
        public bool? ShowBadge { get; set; }

        [XmlIgnore]
        public string MenuInternalId { get; private set; }

        #endregion

        #region Constructors

        public Menu()
        {
            SubMenus = new ChildItemCollection<Menu>(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the Index with other menu object, to sort the menus array.
        /// </summary>
        /// <param name="obj">The menu object to compare to.</param>
        /// <returns>A signed number indicating the relative values of this instance and value. Return
        ///     Value Description Less than zero: This instance is less than value. Zero: This
        ///     instance is equal to value. Greater than zero: This instance is greater than value.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is Menu objMenu)
            {
                return (Index ?? 0).CompareTo(objMenu.Index ?? 0);
            }
            throw new ArgumentException("Object is not a menu.");
        }

        #endregion
    }
}