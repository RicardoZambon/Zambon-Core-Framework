using Zambon.Core.Database;
using Zambon.Core.Module.BusinessObjects;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.DetailViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Xml.Navigation
{
    public class Menu : XmlNode, IComparable, ICloneable
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }


        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("ViewID")]
        public string ViewID { get; set; }

        [XmlIgnore]
        public BaseView View { get; private set; }

        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        [XmlAttribute("ReportPath")]
        public string ReportPath { get; set; }

        [XmlAttribute("URL")]
        public string URL { get; set; }


        [XmlAttribute("ShowBadge")]
        public string BoolShowBadge { get; set; }
        [XmlIgnore]
        public bool ShowBadge { get { return bool.Parse(BoolShowBadge?.ToLower() ?? "false"); } }

        [XmlIgnore]
        public bool IsDropdown { get; set; }


        [XmlElement("Menu")]
        public Menu[] SubMenus { get; set; }


        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            if (SubMenus != null && SubMenus.Length > 0)
                Array.Sort(SubMenus);

            switch (Type)
            {
                case "ListView":
                    View = app.Views.ListViews.FirstOrDefault(x => x.ViewId == ViewID);
                    break;
                case "DetailView":
                    View = app.Views.DetailViews.FirstOrDefault(x => x.ViewId == ViewID);

                    if (string.IsNullOrWhiteSpace(ControllerName))
                        ControllerName = ((DetailView)View).ControllerName;

                    break;
            }

            if (string.IsNullOrWhiteSpace(DisplayName))
                DisplayName = View?.Title;

            if (string.IsNullOrWhiteSpace(Icon))
                Icon = View?.Icon;

            base.OnLoading(app, ctx);
        }

        #endregion

        #region Methods

        public int CompareTo(object obj)
        {
            if (obj is Menu)
                return Index.CompareTo(((Menu)obj).Index);
            throw new ArgumentException("Object is not a menu.");
        }

        public bool UserHasAccess(IUsers _user)
        {
            switch (Type)
            {
                case "ListView":
                    if (!string.IsNullOrEmpty(Id) && !_user.UserHasAccessToMenuID(Id, Type))
                        return false;

                    var entity = View?.GetEntityTypeName();
                    return _user.UserHasAccessToType(entity, 3 /*Navigate*/);
                default:
                    return !string.IsNullOrEmpty(Id) && _user.UserHasAccessToMenuID(Id, Type);
            }
        }

        #endregion

    }
}