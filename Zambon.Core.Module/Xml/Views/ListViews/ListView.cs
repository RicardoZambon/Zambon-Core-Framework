using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Module.ExtensionMethods;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.ListViews.Paint;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    /// <summary>
    /// Represents a node <ListView></ListView> from XML Application Model.
    /// </summary>
    public class ListView : BaseListView, IViewControllerAction, IViewButtons, IViewSubViews
    {

        /// <summary>
        /// The ControllerName attribute from XML. Define the default controller name to be used within this view, by default will use the same as set in EntityType.
        /// </summary>
        [XmlAttribute("ControllerName")]
        public string ControllerName { get; set; }

        /// <summary>
        /// The ActionName attribute from XML. Define the action name to be used for this view.
        /// </summary>
        [XmlAttribute("ActionName")]
        public string ActionName { get; set; }

        /// <summary>
        /// The CanEdit attribute from XML. Indicates if the list view should be editable or not.
        /// </summary>
        [XmlAttribute("CanEdit"), Browsable(false)]
        public string BoolCanEdit
        {
            get { return CanEdit?.ToString(); }
            set { if (value != null) { bool.TryParse(value, out bool canEdit); CanEdit = canEdit; } }
        }
        /// <summary>
        /// The CanEdit attribute from XML. Indicates if the list view should be editable or not.
        /// </summary>
        [XmlIgnore]
        public bool? CanEdit { get; set; }

        /// <summary>
        /// The EditModalId attribute from XML. ID of the ModalView should be used when editting records. By default will search for DetailViews with the same Type.
        /// </summary>
        [XmlAttribute("EditModalId")]
        public string EditModalId { get; set; }


        /// <summary>
        /// The MessageOnEmpty attribute from XML. Message to show when no records are found.
        /// </summary>
        [XmlAttribute("MessageOnEmpty")]
        public string MessageOnEmpty { get; set; }


        /// <summary>
        /// The ShowPagination attribute from XML. Indicates if should display or not a pagiation.
        /// </summary>
        [XmlAttribute("ShowPagination")]
        public string BoolShowPagination { get; set; }
        /// <summary>
        /// The ShowPagination attribute from XML. Indicates if should display or not a pagiation.
        /// </summary>
        [XmlIgnore]
        public bool ShowPagination { get { return bool.Parse(BoolShowPagination?.ToLower() ?? "false"); } }


        /// <summary>
        /// The EditModalParameters attribute from XML. Passes the modal parameters arguments when editing an object. By defualt will use object=[ID].
        /// </summary>
        [XmlAttribute("EditModalParameters")]
        public string EditModalParameters { get; set; }


        /// <summary>
        /// List all buttons.
        /// </summary>
        [XmlIgnore]
        public Button[] Buttons { get { return _Buttons?.Button; } }
        
        /// <summary>
        /// List all paint options and their conditions.
        /// </summary>
        [XmlIgnore]
        public PaintOption[] PaintOptions { get { return _PaintOptions?.PaintOption; } }

        /// <summary>
        /// The SubViews element from XML.
        /// </summary>
        [XmlElement("SubViews")]
        public SubViews.SubViews SubViews { get; set; }


        /// <summary>
        /// The PaginationOptions element from XML. Indicates how the pagination should behave.
        /// </summary>
        [XmlElement("PaginationOptions")]
        public PaginationOptions PaginationOptions { get; set; }

        /// <summary>
        /// The Buttons element from XML.
        /// </summary>
        [XmlElement("Buttons"), Browsable(false)]
        public Buttons.Buttons _Buttons { get; set; }
        
        /// <summary>
        /// The PaintOptions element from XML.
        /// </summary>
        [XmlElement("PaintOptions"), Browsable(false)]
        public PaintOptionsArray _PaintOptions { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public IDictionary<string, object> CustomParameters { get; protected set; } = new Dictionary<string, object>();

        /// <summary>
        /// The EditModalID modal object.
        /// </summary>
        [XmlIgnore]
        public DetailModal EditModal { get; protected set; }

        
        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (string.IsNullOrWhiteSpace(ControllerName) && !string.IsNullOrWhiteSpace(Entity.DefaultController))
                ControllerName = Entity.DefaultController;

            if (string.IsNullOrWhiteSpace(ActionName))
                ActionName = app.ModuleConfiguration.ListViewDefaults.DefaultAction;

            if (string.IsNullOrWhiteSpace(EditModalParameters))
                EditModalParameters = app.ModuleConfiguration.ListViewDefaults.DefaultEditModalParameter;
            
            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
            {
                for (var d = 0; d < SubViews.DetailViews.Length; d++)
                    SubViews.DetailViews[d].ParentViewId = ViewId;
                    
                if (string.IsNullOrWhiteSpace(EditModalId))
                    EditModalId = Array.Find(SubViews.DetailViews, x => x.View.Type == Type)?.Id;

                EditModal = Array.Find(SubViews.DetailViews, x => x.Id == EditModalId);
            }

            if ((SubViews?.LookupViews?.Length ?? 0) > 0)
                for (var l = 0; l < SubViews.LookupViews.Length; l++)
                    SubViews.LookupViews[l].ParentViewId = ViewId;
                 
            if ((SubViews?.SubListViews?.Length ?? 0) > 0)
                for (var s = 0; s < SubViews.SubListViews.Length; s++)
                    SubViews.SubListViews[s].ParentViewId = ViewId;
            

            if (string.IsNullOrWhiteSpace(BoolCanEdit))
                BoolCanEdit = app.ModuleConfiguration?.ListViewDefaults?.BoolCanEdit;

            if (string.IsNullOrWhiteSpace(BoolShowPagination))
                BoolShowPagination = app.ModuleConfiguration?.ListViewDefaults?.BoolShowPagination;
            
            if ((Buttons?.Length ?? 0) > 0)
            {
                for (var b = 0; b < Buttons.Length; b++)
                    OnLoadingSubButtons(Buttons[b]);
            }
            
            if (PaginationOptions == null)
                PaginationOptions = new PaginationOptions();

            if (PaginationOptions.PageSize == null)
                PaginationOptions.PageSize = app.ModuleConfiguration.ListViewDefaults.PageSize.Value;

            if (PaginationOptions.PagesToShow == null)
                PaginationOptions.PagesToShow = app.ModuleConfiguration.ListViewDefaults.PagesToShow.Value;
        }

        internal override void OnLoadingUserModel(Application app, CoreDbContext ctx)
        {
            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
                EditModal = Array.Find(SubViews.DetailViews, x => x.Id == EditModalId);

            base.OnLoadingUserModel(app, ctx);
        }

        private void OnLoadingSubButtons(Button button)
        {
            if (string.IsNullOrWhiteSpace(button.ControllerName))
                button.ControllerName = ControllerName;

            if (button.IsApplicable("Inline"))
            {
                if (string.IsNullOrWhiteSpace(button.LoadingContainer))
                    button.LoadingContainer = "body";
            }
            else if (button.IsApplicable("Toolbar"))
            {
                if (string.IsNullOrWhiteSpace(button.LoadingContainer))
                    button.LoadingContainer = "body";
            }

            if ((button.SubButtons?.Length ?? 0) > 0)
                for (var b = 0; b < button.SubButtons.Length; b++)
                    OnLoadingSubButtons(button.SubButtons[b]);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the ListView contents to the current page.
        /// </summary>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="currentPage">Current page should be displayed. By default is "1".</param>
        /// <param name="searchOptions">If applyng search, otherwise null.</param>
        public void SetCurrentPage(ApplicationService app, CoreDbContext ctx, int currentPage = 1, SearchOptions searchOptions = null)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(SetCurrentPage) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx, currentPage, searchOptions });
        }
        /// <summary>
        /// Set the ListView contents to the current page.
        /// </summary>
        /// <typeparam name="T">The type of the ListView.</typeparam>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="currentPage">Current page should be displayed. By default is "1".</param>
        /// <param name="searchOptions">If applyng search, otherwise null.</param>
        public void SetCurrentPage<T>(ApplicationService app, CoreDbContext ctx, int currentPage = 1, SearchOptions searchOptions = null) where T : class
        {
            //Todo: Validate if still needed. GC.Collect();
            var list = GetPopulatedView<T>(app, ctx, searchOptions);

            if (ShowPagination && PaginationOptions != null)
            {
                PaginationOptions.ChangePage(currentPage, list.Count());
                if (list.Count() > PaginationOptions.PageSize)
                    list = list.Skip((PaginationOptions.PageSize ?? 0) * ((PaginationOptions.ActualPage ?? 0) - 1)).Take(PaginationOptions.PageSize ?? 0);
            }

            CurrentObject = null;
            ItemsCollection = list;
        }


        /// <summary>
        /// Set the ListView contents to the value of an entity.
        /// </summary>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="entity">The entity that has the collection.</param>
        /// <param name="collection">The collection name.</param>
        public void SetItemsCollection(ApplicationService app, CoreDbContext ctx, object entity, string collection)
        {
            SetItemsCollection(app, ctx, (IEnumerable)entity.GetType().GetProperty(collection).GetValue(entity));
        }
        /// <summary>
        /// Set the ListView contents to the value of an Enumerable.
        /// </summary>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="collection">The collection Enumerable value.</param>
        public void SetItemsCollection(ApplicationService app, CoreDbContext ctx, IEnumerable collection)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(SetItemsCollection) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx, collection });
        }
        /// <summary>
        /// Set the ListView contents to the value of an Enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the Enumerable.</typeparam>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <param name="collection">The collection Enumerable value.</param>
        public void SetItemsCollection<T>(ApplicationService app, CoreDbContext ctx, IEnumerable<T> collection) where T : class
        {
            IQueryable<T> list = null;

            if (collection != null)
            {
                if (typeof(T).ImplementsInterface<IEntity>())
                {
                    list = collection.AsQueryable();

                    if (!string.IsNullOrEmpty(FromSql))
                        list = list.FromSql(FromSql);

                    if (typeof(T).IsSubclassOf(typeof(DBObject)))
                        list = list.Where("!IsDeleted");
                }
                else if (typeof(T).ImplementsInterface<IQuery>())
                {
                    if (!string.IsNullOrWhiteSpace(FromSql))
                        list = ctx.Set<T>().FromSql(FromSql);
                    else
                        throw new ApplicationException($"The ListView \"{ViewId}\" has an entity \"{Entity}\" that implements the IQuery interface and is mandatory inform the attribute FromSql in ListView or Entity definition.");
                }
                else
                    throw new ApplicationException($"The ListView \"{ViewId}\" entity \"{Entity}\" does not have implemented the interface IEntity not IQuery.");

                if (!string.IsNullOrWhiteSpace(Sort))
                    list = list.OrderBy(Sort);
            }

            CurrentObject = null;
            ItemsCollection = list;
        }

        /// <summary>
        /// Get the ListView items Count().
        /// </summary>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <returns>Returns the Count() of items as integer.</returns>
        public int GetItemsCount(ApplicationService app, CoreDbContext ctx)
        {
            return (int)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetItemsCount) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx });
        }
        /// <summary>
        /// Get the ListView items Count().
        /// </summary>
        /// /// <typeparam name="T">The type of the ListView.</typeparam>
        /// <param name="app">The application service.</param>
        /// <param name="ctx">The CoreDbContext service.</param>
        /// <returns>Returns the Count() of items as integer.</returns>
        public int GetItemsCount<T>(ApplicationService app, CoreDbContext ctx) where T : class
        {
            return GetItemsList<T>(app, ctx).Count();
        }


        /// <summary>
        /// Retrieves a SubView using the SubView Id.
        /// </summary>
        /// <param name="Id">The Id of the SubView.</param>
        /// <returns>If found, return the SubView instance; Otherwise, return null.</returns>
        public BaseSubView GetSubView(string Id)
        {
            BaseSubView view = null;
            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
                view = Array.Find(SubViews.DetailViews, m => m.Id == Id);

            if (view == null && (SubViews?.LookupViews?.Length ?? 0) > 0)
                view = Array.Find(SubViews.LookupViews, m => m.Id == Id);

            if (view == null && (SubViews?.SubListViews?.Length ?? 0) > 0)
            {
                view = Array.Find(SubViews.SubListViews, m => m.Id == Id);
                if (view == null)
                    for (var s = 0; s < SubViews.SubListViews.Length; s++)
                    {
                        view = SubViews.SubListViews[s].ListView.GetSubView(Id);
                        if (view != null)
                            return view;
                    }
            }

            return view;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetApplicableBackColor(ExpressionsService service, object obj)
        {
            if (PaintOptions != null)
                return string.Join(' ', service.GetApplicableItems(PaintOptions, obj).Select(x => x.BackColor));
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetApplicableForeColor(ExpressionsService service, object obj)
        {
            if (PaintOptions != null)
                return string.Join(' ', service.GetApplicableItems(PaintOptions, obj).Select(x => x.ForeColor));
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetApplicableCssClass(ExpressionsService service, object obj)
        {
            if (PaintOptions != null)
                return string.Join(' ', service.GetApplicableItems(PaintOptions, obj).Select(x => x.CssClass));
            return string.Empty;
        }

        #endregion

    }
}