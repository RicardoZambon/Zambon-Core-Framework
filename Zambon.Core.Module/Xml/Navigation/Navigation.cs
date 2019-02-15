using Zambon.Core.Database;
using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Navigation
{
    public class Navigation : XmlNode
    {

        #region Properties

        [XmlElement("Menu")]
        public Menu[] Menus { get; set; }

        #endregion

        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            if ((Menus?.Length ?? 0) > 0)
                Array.Sort(Menus);

            base.OnLoading(app, ctx);
        }

        #endregion

        #region Methods

        //public static void SetDisplayNameIconMenus(Menu[] _menus, Views.Views _views)
        //{
        //    for (var i = 0; i < _menus.Length; i++)
        //    {
        //        if (_menus[i].Type == "ListView")
        //        {
        //            if (string.IsNullOrWhiteSpace(_menus[i].DisplayName)) _menus[i].DisplayName = _menus[i].GetViewTitle(_views);
        //            if (string.IsNullOrWhiteSpace(_menus[i].Icon)) _menus[i].Icon = _menus[i].GetViewIcon(_views);
        //        }
        //        if (_menus[i].Menus?.Length > 0)
        //            SetDisplayNameIconMenus(_menus[i].Menus, _views);
        //    }
        //}

        //public static void SortMenus(Menu[] _menus, bool isDropdown = false)
        //{
        //    Array.Sort(_menus);
        //    for (var i = 0; i < _menus.Length; i++)
        //    {
        //        _menus[i].IsDropdown = isDropdown;
        //        if (_menus[i].Menus?.Length > 0)
        //            SortMenus(_menus[i].Menus, true);
        //    }
        //}

        #endregion

    }
}