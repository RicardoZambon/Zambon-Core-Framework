using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.DetailViews;

namespace Zambon.Core.Module.Xml.Navigation
{
    public class Menu : XmlNode, IComparable
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { int.TryParse(value, out int index); Index = index; }
        }
        [XmlIgnore]
        public int? Index { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("ViewID")]
        public string ViewID { get; set; }

        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        [XmlAttribute("ReportPath")]
        public string ReportPath { get; set; }

        [XmlAttribute("URL")]
        public string URL { get; set; }
        
        [XmlAttribute("ShowBadge"), Browsable(false)]
        public string BoolShowBadge
        {
            get { return ShowBadge?.ToString(); }
            set { bool.TryParse(value, out bool showBadge); ShowBadge = showBadge; }
        }
        [XmlIgnore]
        public bool? ShowBadge { get; set; }

        [XmlElement("Menu")]
        public Menu[] SubMenus { get; set; }


        [XmlIgnore]
        public BaseView View { get; private set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            if (SubMenus != null && SubMenus.Length > 0)
                Array.Sort(SubMenus);

            View view = null;
            switch (Type)
            {
                case "ListView":
                    view = app.FindListView(ViewID);
                    break;
                case "DetailView":
                    view = app.FindDetailView(ViewID);
                    if (string.IsNullOrWhiteSpace(ControllerName))
                        ControllerName = ((DetailView)View).ControllerName;
                    break;
            }

            if (view != null)
            {
                if (string.IsNullOrWhiteSpace(DisplayName))
                    DisplayName = View?.Title;

                if (string.IsNullOrWhiteSpace(Icon))
                    Icon = View?.Icon;
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

        public int CompareTo(object obj)
        {
            if (obj is Menu objMenu)
                return (Index ?? 0).CompareTo(objMenu.Index ?? 0);
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