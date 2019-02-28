using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;
using Zambon.Core.Database;
using Zambon.Core.Module.Xml.Views.SubViews;
using Zambon.Core.Module.Xml.Navigation;
using Zambon.Core.Module.Xml.Views.ListViews;
using System.Text.RegularExpressions;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Helper;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml;
using Zambon.Core.Module.Services.InstanceObjects;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Interfaces;
using Microsoft.Extensions.Options;

namespace Zambon.Core.Module.Services
{
    public class ApplicationService
    {

        #region Variables

        private readonly CoreDbContext Ctx;

        private readonly ILanguageService LanguageService;

        private readonly ModelService ModelService;

        public readonly ExpressionsService Expressions;

        private readonly IUserService UserService;

        #endregion

        #region Properties

        private Application _Model;
        private Application Model { get { if (_Model == null) _Model = ModelService.GetModel(Ctx, LanguageService.GetCurrentLanguage()); return _Model; } }

        public ApplicationConfigs AppConfigs { get { return ModelService.GetAppSettings(); } }


        public string AppName { get { return Model.Name; } }

        public string AppMenuName { get { return Model.MenuName; } }

        public string AppFullName { get { return Model.FullName; } }

        public string Version { get { return ModelService.AppVersion; } }

        public string Copyright { get { return ModelService.AppCopyright; } }


        public string LoginTheme { get { return Model.ModuleConfiguration?.LoginDefaults?.Theme ?? string.Empty; } }


        private Menu[] _UserMenu;
        public Menu[] UserMenu { get { if (_UserMenu == null || _UserMenu.Length == 0) _UserMenu = GetMenus(); return _UserMenu; } }

        public IUsers CurrentUser { get { return UserService.CurrentUser; } }

        #endregion

        #region Constructors

        public ApplicationService(CoreDbContext ctx, ILanguageService languageService, ModelService modelService, ExpressionsService expressions, IUserService userService)
        {
            Ctx = ctx;
            LanguageService = languageService;
            ModelService = modelService;
            Expressions = expressions;
            UserService = userService;
        }

        #endregion

        #region Methods

        public string GetDefaultProperty(string clrType)
        {
            return Model.FindEntityByClrType(clrType)?.GetDefaultProperty() ?? string.Empty;
        }

        public string GetPropertyDisplayName(string clrType, string propertyName)
        {
            return Model.FindEntityByClrType(clrType)?.GetPropertyDisplayName(propertyName) ?? string.Empty;
        }


        public string GetStaticText(string _key)
        {
            return Model.GetStaticText(_key);
        }


        public BaseView GetBaseView(string _viewId)
        {
            if (GetView(_viewId) is View view)
                return view;

            if (GetLookupView(_viewId) is LookupView lookupView)
                return lookupView;

            return null;
        }

        public View GetView(string _viewId)
        {
            if (!string.IsNullOrWhiteSpace(_viewId))
            {
                if (GetDetailView(_viewId) is DetailView detailView)
                    return detailView;

                if (GetListView(_viewId) is ListView listView)
                    return listView;
            }
            return null;
        }


        public DetailView GetDetailView(string _detailViewId)
        {
            return Model.FindDetailView(_detailViewId);
        }

        public ListView GetListView(string _listViewId)
        {
            return Model.FindListView(_listViewId);
        }

        public LookupView GetLookupView(string _lookupViewId)
        {
            return Model.FindLookupView(_lookupViewId);
        }


        private Menu[] GetMenus(Menu[] _subMenus = null)
        {
            var list = new List<Menu>();
            if (CurrentUser != null)
            {
                var subMenus = _subMenus ?? Model.Navigation;
                for (var i = 0; i < subMenus.Count(); i++)
                {
                    var menu = subMenus[i];
                    if (subMenus[i].SubMenus?.Length > 0)
                    {
                        var childSubMenus = GetMenus(subMenus[i].SubMenus);
                        if (childSubMenus?.Length > 0)
                        {
                            menu.SubMenus = childSubMenus;
                            list.Add(menu);
                        }
                    }
                    else if (menu.Type == "Separator")
                    {
                        if (list.Count() > 0 && list[list.Count - 1].Type != "Separator")
                            list.Add(menu);
                    }
                    else if (menu.UserHasAccess(CurrentUser))
                        list.Add(menu);
                }
            }
            return list.ToArray();
        }

        #endregion

    }
}