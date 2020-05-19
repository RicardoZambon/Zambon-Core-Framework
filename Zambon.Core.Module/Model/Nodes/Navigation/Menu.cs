using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Navigation
{
    public abstract class Menu<TMenu> : SerializeNodeBase, IMenu<TMenu>
        where TMenu : class, IMenu<TMenu>
    {
        #region XML Attributes 

        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        [XmlAttribute]
        public string Icon { get; set; }

        [XmlAttribute(nameof(Index)), Browsable(false)]
        public string IntIndex
        {
            get { return Index?.ToString(); }
            set { if (value != null) { int.TryParse(value, out int index); Index = index; } }
        }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string ViewID { get; set; }

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

        #region XML Arrays

        [XmlArray, XmlArrayItem(nameof(Menu))]
        public ChildItemCollection<TMenu> SubMenus { get; set; }

        #endregion

        #region Properties

        [XmlIgnore]
        public int? Index { get; set; }

        [XmlIgnore]
        public bool? ShowBadge { get; set; }

        #endregion

        #region Constructors

        public Menu()
        {
            SubMenus = new ChildItemCollection<TMenu>(this);
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
            if (obj is Menu<TMenu> objMenu)
            {
                return (Index ?? 0).CompareTo(objMenu.Index ?? 0);
            }
            throw new ArgumentException("Object is not a menu.");
        }

        #endregion
    }

    public sealed class Menu : Menu<Menu>
    {

    }
}