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
using Zambon.Core.Module.BusinessObjects;
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

        private readonly CoreContext _ctx;

        private readonly ModelService _modelService;

        private readonly GlobalExpressionsService _expressions;
        public GlobalExpressionsService Expressions { get { return _expressions; } }

        private readonly ICurrentUserService _currentUser;


        private Application _model;
        private Application Model {
            get
            {
                if (_model == null)
                    _model = (Application)_modelService.GetModel().Clone();
                return _model;
            }
        }

        public IDictionary<string, ListViewInstance> ListViewInstances { get; set; }
        public IDictionary<string, LookUpViewInstance> LookUpViewInstances { get; set; }
        public IDictionary<string, DetailViewInstance> DetailViewInstances { get; set; }

        #endregion

        #region Properties

        private Menu[] _UserMenu;
        public Menu[] UserMenu
        {
            get
            {
                _currentUser.CheckUserChanged();
                if (_UserMenu == null || _UserMenu.Length == 0) _UserMenu = GetMenus();
                return _UserMenu;
            }
        }

        public IUsers CurrentUser { get { return _currentUser.CurrentUser; } }

        #endregion

        #region Constructors

        public ApplicationService(CoreContext ctx, ModelService modelService, GlobalExpressionsService expressions, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _modelService = modelService;
            _expressions = expressions;
            _currentUser = currentUserService;

            ListViewInstances = new Dictionary<string, ListViewInstance>();
            LookUpViewInstances = new Dictionary<string, LookUpViewInstance>();
            DetailViewInstances = new Dictionary<string, DetailViewInstance>();
        }

        #endregion

        #region Methods

        public string GetAppName()
        {
            return Model.Name;
        }

        public string GetAppMenuName()
        {
            return Model.MenuName;
        }

        public string GetAppFullName()
        {
            return Model.FullName;
        }

        public string GetAppVersion()
        {
            return _modelService.GetAppVersion();
        }

        public string GetAppCopyright()
        {
            return _modelService.GetAppCopyright();
        }

        public AppSettings GetAppSettings()
        {
            return _modelService.GetAppSettings();
        }


        public string GetLoginTheme()
        {
            return Model.ModuleConfiguration?.LoginDefaults?.Theme ?? string.Empty;
        }


        public string GetPropertyDisplayName(string clrType, string propertyName)
        {
            return Model.FindEntityByClrType(clrType)?.GetProperty(propertyName)?.DisplayName ?? string.Empty;
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
                var subMenus = _subMenus ?? Model.Navigation.Menus;
                for (var i = 0; i < subMenus.Count(); i++)
                {
                    var menu = (Menu)subMenus[i].Clone();
                    if (subMenus[i].SubMenus?.Length > 0)
                    {
                        var childSubMenus = GetMenus(subMenus[i].SubMenus);
                        if (childSubMenus?.Length > 0)
                        {
                            menu.SubMenus = childSubMenus;

                            //if (menu.ShowBadge)
                            //    menu.BadgeCount = childSubMenus.FirstOrDefault(x => x.ShowBadge)?.BadgeCount ?? -1;

                            list.Add(menu);
                        }
                    }
                    else if (menu.Type == "Separator")
                    {
                        if (list.Count() > 0 && list[list.Count - 1].Type != "Separator")
                            list.Add(menu);
                    }
                    else if (menu.UserHasAccess(CurrentUser))//, Model.Views))
                    {
                        //if (menu.ShowBadge)
                            //if (menu.Type == "ListView")
                                //menu.BadgeCount = Model.FindListView(menu.ViewID).GetTotalItems(_ctx, _expression);
                        list.Add(menu);
                    }
                }
            }
            return list.ToArray();
        }

        public LookupModal LoadLookupView(LookupModal _modalLookup)
        {
            //if (_modalLookup.View == null)
            //    _modalLookup.View = Model.GetModel().FindLookupView(_modalLookup.ViewId);
            //else
            //    _modalLookup.View = Merge.MergeObject(Model.GetModel().FindLookupView(_modalLookup.ViewId), _modalLookup.View);

            ////_modalLookup.View.PopulateView(_ctx);
            //return _modalLookup;
            return null;
        }

        #endregion

        #region ListView Instance

        public void ClearListViewCurrentObject(string listViewId)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                ListViewInstances[listViewId].CurrentObject = null;
        }
        public void SetListViewCurrentObject(string listViewId, object currentObject)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                ListViewInstances[listViewId].CurrentObject = currentObject;
            else
                ListViewInstances.Add(listViewId, new ListViewInstance(currentObject));
        }
        public object GetListViewCurrentObject(string listViewId)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                return ListViewInstances[listViewId].CurrentObject;
            return null;
        }

        public void SetListViewItemsCollection(string listViewId, IQueryable collection)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                ListViewInstances[listViewId].ItemsCollection = collection;
            else
                ListViewInstances.Add(listViewId, new ListViewInstance(collection));
        }
        public IQueryable GetListViewItemsCollection(string listViewId)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                return ListViewInstances[listViewId].ItemsCollection;
            return null;
        }

        public void SetListViewSearchOptions(string listViewId, SearchOptions searchOptions)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                ListViewInstances[listViewId].SearchOptions = searchOptions;
            else if (searchOptions != null)
                ListViewInstances.Add(listViewId, new ListViewInstance(searchOptions));
        }
        public SearchOptions GetListViewSearchOptions(string listViewId)
        {
            if (ListViewInstances.ContainsKey(listViewId))
                return ListViewInstances[listViewId].SearchOptions;
            return null;
        }

        #endregion

        #region LookUpView Instance

        public void ClearLookUpViewCurrentObject(string lookUpViewId)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                LookUpViewInstances[lookUpViewId].CurrentObject = null;
        }
        public void SetLookUpViewCurrentObject(string lookUpViewId, BaseDBObject currentObject)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                LookUpViewInstances[lookUpViewId].CurrentObject = currentObject;
            else
                LookUpViewInstances.Add(lookUpViewId, new LookUpViewInstance(currentObject));
        }
        public BaseDBObject GetLookUpViewCurrentObject(string lookUpViewId)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                return LookUpViewInstances[lookUpViewId].CurrentObject;
            return null;
        }

        public void SetLookUpViewItemsCollection(string lookUpViewId, IQueryable<BaseDBObject> collection)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                LookUpViewInstances[lookUpViewId].ItemsCollection = collection;
            else
                LookUpViewInstances.Add(lookUpViewId, new LookUpViewInstance(collection));
        }
        public IQueryable<BaseDBObject> GetLookUpViewItemsCollection(string lookUpViewId)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                return LookUpViewInstances[lookUpViewId].ItemsCollection;
            return null;
        }

        public void SetLookUpViewSearchOptions(string lookUpViewId, SearchOptions searchOptions)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                LookUpViewInstances[lookUpViewId].SearchOptions = searchOptions;
            else if (searchOptions != null)
                LookUpViewInstances.Add(lookUpViewId, new LookUpViewInstance(searchOptions));
        }
        public SearchOptions GetLookUpViewSearchOptions(string lookUpViewId)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                return LookUpViewInstances[lookUpViewId].SearchOptions;
            return null;
        }

        public void SetLookUpViewPostBackOptions(string lookUpViewId, PostBackOptions postBackOptions)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                LookUpViewInstances[lookUpViewId].PostBackOptions = postBackOptions;
            else if (postBackOptions != null)
                LookUpViewInstances.Add(lookUpViewId, new LookUpViewInstance(postBackOptions));
        }
        public PostBackOptions GetLookUpViewPostBackOptions(string lookUpViewId)
        {
            if (LookUpViewInstances.ContainsKey(lookUpViewId))
                return LookUpViewInstances[lookUpViewId].PostBackOptions;
            return null;
        }

        #endregion

        #region DetailView Instance

        public void ClearDetailViewCurrentObject(string detailViewId)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                DetailViewInstances[detailViewId].CurrentObject = null;
        }
        public void SetDetailViewCurrentObject(string detailViewId, object currentObject)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                DetailViewInstances[detailViewId].CurrentObject = currentObject;
            else
                DetailViewInstances.Add(detailViewId, new DetailViewInstance(currentObject));
        }
        public object GetDetailViewCurrentObject(string detailViewId)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                return DetailViewInstances[detailViewId].CurrentObject;
            return null;
        }

        public void SetDetailViewCurrentView(string detailViewId, string currentView)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                DetailViewInstances[detailViewId].CurrentView = currentView;
            else
                DetailViewInstances.Add(detailViewId, new DetailViewInstance(currentView));
        }
        public string GetDetailViewCurrentView(string detailViewId)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                return DetailViewInstances[detailViewId].CurrentView;
            return null;
        }

        public void SetDetailViewTitle(string detailViewId, string title)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                DetailViewInstances[detailViewId].Title = title;
            else
                DetailViewInstances.Add(detailViewId, new DetailViewInstance() { Title = title });
        }
        public string GetDetailViewTitle(string detailViewId)
        {
            if (DetailViewInstances.ContainsKey(detailViewId))
                return DetailViewInstances[detailViewId].Title;
            return null;
        }

        #endregion

    }
}