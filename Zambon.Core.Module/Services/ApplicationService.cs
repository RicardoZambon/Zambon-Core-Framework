using System.Collections.Generic;
using System.Linq;
using Zambon.Core.Database;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Xml;
using Zambon.Core.Module.Xml.Navigation;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;

namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// The main service used in application, is registered under Scoped lifecycle.
    /// </summary>
    public class ApplicationService
    {

        #region Variables

        private readonly CoreDbContext Ctx;

        private readonly ILanguageService LanguageService;

        private readonly ModelService ModelService;

        private readonly ExpressionsService ExpressionsService;

        private readonly IUserService UserService;

        #endregion

        #region Properties

        private Application _Model;
        private Application Model { get { if (_Model == null) _Model = ModelService.GetModel(Ctx, LanguageService.GetCurrentLanguage()); return _Model; } }


        /// <summary>
        /// Returns the current instance of the AppSettings.json file.
        /// </summary>
        public ApplicationConfigs AppConfigs { get { return ModelService.GetAppSettings(); } }


        /// <summary>
        /// The name of the application, from ApplicationModel.xml <Application Name=""></Application>.
        /// </summary>
        public string AppName { get { return Model.Name; } }

        /// <summary>
        /// The name of the application used in menu (If null will use the application name), from ApplicationModel.xml <Application MenuName=""></Application>.
        /// </summary>
        public string AppMenuName { get { return Model.MenuName; } }

        /// <summary>
        /// The full name of the application used in home page, from ApplicationModel.xml <Application FullName=""></Application>.
        /// </summary>
        public string AppFullName { get { return Model.FullName; } }

        /// <summary>
        /// The current version of the application, from the startup project Package > Package version.
        /// </summary>
        public string Version { get { return ModelService.AppVersion; } }

        /// <summary>
        /// The copyright of the application, from the startup project Package > Copyright.
        /// </summary>
        public string Copyright { get { return ModelService.AppCopyright; } }


        /// <summary>
        /// The login theme used in Login page.
        /// </summary>
        public string LoginTheme { get { return Model.ModuleConfiguration?.LoginDefaults?.Theme ?? string.Empty; } }


        private Menu[] _UserMenu;
        /// <summary>
        /// The user menu array, already filtered accordingly to the available options.
        /// </summary>
        public Menu[] UserMenu { get { if ((_UserMenu?.Length ?? 0) == 0) _UserMenu = RefreshMenus(); return _UserMenu; } }

        /// <summary>
        /// Current active user object from UserService.
        /// </summary>
        public IUsers CurrentUser { get { return UserService.CurrentUser; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for the ApplicationService.
        /// </summary>
        /// <param name="ctx">Database instance service.</param>
        /// <param name="languageService">Current language service.</param>
        /// <param name="modelService">ApplicationModel.xml service.</param>
        /// <param name="expressionsService">Expressions service.</param>
        /// <param name="userService">Current active user service.</param>
        public ApplicationService(CoreDbContext ctx, ILanguageService languageService, ModelService modelService, ExpressionsService expressionsService, IUserService userService)
        {
            Ctx = ctx;
            LanguageService = languageService;
            ModelService = modelService;
            ExpressionsService = expressionsService;
            UserService = userService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the current used instance of the ExpressionsService.
        /// </summary>
        /// <returns></returns>
        public ExpressionsService GetExpressionsService()
        {
            return ExpressionsService;
        }


        /// <summary>
        /// Search for the default property from the entity type.
        /// </summary>
        /// <param name="clrType">The EntityType ClrType to search for.</param>
        /// <returns>Return the System.ComponentModel.DefaultProperty attribute value, string.Empty if not found any default property defined.</returns>
        public string GetDefaultProperty(string clrType)
        {
            return Model.FindEntityByClrType(clrType)?.GetDefaultProperty() ?? string.Empty;
        }

        /// <summary>
        /// Search for the display name of a property from a entity type.
        /// </summary>
        /// <param name="clrType">The EntityType ClrType to search for.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>Return the System.ComponentModel.DataAnnotations.DisplayAttribute value, string.Empty if not found the display attribute.</returns>
        public string GetPropertyDisplayName(string clrType, string propertyName)
        {
            return Model.FindEntityByClrType(clrType)?.GetPropertyDisplayName(propertyName) ?? string.Empty;
        }


        /// <summary>
        /// Search all Static Texts using the Key property.
        /// </summary>
        /// <param name="key">The static text key to search.</param>
        /// <returns>Return the Static Text value string, string.Empty if not found.</returns>
        public string GetStaticText(string key)
        {
            return Model.GetStaticText(key);
        }


        /// <summary>
        /// Search DetailViews, ListViews and LookUpViews using the Id property.
        /// </summary>
        /// <param name="viewId">The id of the view to search.</param>
        /// <returns>Return the BaseView object, null if not found.</returns>
        public BaseView GetView(string viewId)
        {
            if (!string.IsNullOrWhiteSpace(viewId))
            {
                if (GetDetailView(viewId) is DetailView detailView)
                    return detailView;

                if (GetListView(viewId) is ListView listView)
                    return listView;

                if (GetLookupView(viewId) is LookupView lookupView)
                    return lookupView;
            }
            return null;
        }

        /// <summary>
        /// Search all DetailViews using the Id property.
        /// </summary>
        /// <param name="detailViewId">The id of the view to search.</param>
        /// <returns>Return the DetailView object, null if not found.</returns>
        public DetailView GetDetailView(string detailViewId)
        {
            return Model.FindDetailView(detailViewId);
        }

        /// <summary>
        /// Search all ListViews using the Id property.
        /// </summary>
        /// <param name="listViewId">The id of the view to search.</param>
        /// <returns>Return the ListView object, null if not found.</returns>
        public ListView GetListView(string listViewId)
        {
            return Model.FindListView(listViewId);
        }

        /// <summary>
        /// Search all LookUpViews using the Id property.
        /// </summary>
        /// <param name="lookUpViewId">The id of the view to search.</param>
        /// <returns>Return the LookUpView object, null if not found.</returns>
        public LookupView GetLookupView(string lookUpViewId)
        {
            return Model.FindLookupView(lookUpViewId);
        }


        private Menu[] RefreshMenus(Menu[] subMenus = null)
        {
            if (CurrentUser != null)
            {
                var menus = (subMenus ?? Model.Navigation).ToList();
                for (var m = 0; m < menus.Count(); m++)
                {
                    var menu = menus[m];
                    var removeItem = false;

                    if (menu.Index >= 0)
                    {
                        if ((menu.SubMenus?.Length ?? 0) > 0)
                        {
                            menu.SubMenus = RefreshMenus(menu.SubMenus);
                            if (menu.SubMenus.Length == 0)
                                removeItem = true;
                        }
                        else if (menu.Type == "Separator")
                        {
                            if (m == 0 || menus[menus.Count - 1].Type == "Separator")
                                removeItem = true;
                        }
                        else if (!menu.UserHasAccess(CurrentUser))
                            removeItem = true;
                    }
                    else
                        removeItem = true;

                    if (removeItem)
                    {
                        menus.Remove(menu);
                        m--;
                    }
                }
            }
            return new Menu[0];
        }

        #endregion

    }
}