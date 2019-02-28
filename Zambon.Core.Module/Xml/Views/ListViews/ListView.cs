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
using Zambon.Core.Module.Helper;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.Buttons;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.ListViews.Paint;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    public class ListView : View, ICriteria
    {

        [XmlAttribute("CanEdit"), Browsable(false)]
        public string BoolCanEdit
        {
            get { return CanEdit?.ToString(); }
            set { bool.TryParse(value, out bool canEdit); CanEdit = canEdit; }
        }
        [XmlIgnore]
        public bool? CanEdit { get; set; }

        [XmlAttribute("EditModalId")]
        public string EditModalId { get; set; }


        [XmlAttribute("Criteria")]
        public string Criteria { get; set; }

        [XmlAttribute("CriteriaArguments")]
        public string CriteriaArguments { get; set; }

        [XmlAttribute("FromSql")]
        public string FromSql { get; set; }

        [XmlAttribute("Sort")]
        public string Sort { get; set; }


        [XmlAttribute("MessageOnEmpty")]
        public string MessageOnEmpty { get; set; }


        [XmlAttribute("ShowPagination")]
        public string BoolShowPagination { get; set; }
        [XmlIgnore]
        public bool ShowPagination { get { return bool.Parse(BoolShowPagination?.ToLower() ?? "false"); } }


        [XmlAttribute("EditModalParameters")]
        public string EditModalParameters { get; set; }


        [XmlIgnore]
        public SearchProperty[] SearchProperties { get { return _SearchProperties?.SearchProperty; } }

        [XmlIgnore]
        public Column[] Columns { get { return _Columns?.Column; } }

        [XmlIgnore]
        public PaintOption[] PaintOptions { get { return _PaintOptions?.PaintOption; } }

        [XmlElement("PaginationOptions")]
        public PaginationOptions PaginationOptions { get; set; }


        [XmlElement("SearchProperties"), Browsable(false)]
        public SearchPropertiesArray _SearchProperties { get; set; }

        [XmlElement("Columns"), Browsable(false)]
        public ColumnsArray _Columns { get; set; }

        [XmlElement("PaintOptions"), Browsable(false)]
        public PaintOptionsArray _PaintOptions { get; set; }


        [XmlIgnore]
        public IDictionary<string, object> CustomParameters { get; private set; } = new Dictionary<string, object>();

        [XmlIgnore]
        public DetailModal EditModal { get; private set; }


        [XmlIgnore]
        public SearchOptions SearchOptions { get; private set; }

        [XmlIgnore]
        public IQueryable ItemsCollection { get; private set; }


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (string.IsNullOrWhiteSpace(ControllerName))
                ControllerName = Entity.DefaultController;

            if (string.IsNullOrWhiteSpace(ActionName))
                ActionName = app.ModuleConfiguration.ListViewDefaults.DefaultAction;

            if (string.IsNullOrWhiteSpace(EditModalParameters))
                EditModalParameters = app.ModuleConfiguration.ListViewDefaults.DefaultEditModalParameter;

            if (string.IsNullOrWhiteSpace(Sort))
                Sort = Entity.GetDefaultProperty();

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

            if ((SearchProperties?.Length ?? 0) > 0)
            {
                Array.Sort(SearchProperties);
                for (var s = 0; s < SearchProperties.Length; s++)
                {
                    var property = Entity?.GetPropertyDisplayName(SearchProperties[s].PropertyName);
                    if (string.IsNullOrWhiteSpace(SearchProperties[s].DisplayName))
                        SearchProperties[s].DisplayName = property;
                    SearchProperties[s].OnLoadingXml(app, ctx);
                }
            }

            if ((Buttons?.Length ?? 0) > 0)
            {
                for (var b = 0; b < Buttons.Length; b++)
                    OnLoadingSubButtons(Buttons[b]);
            }

            if ((Columns?.Length ?? 0) > 0)
            {
                Array.Sort(Columns);
                for (var s = 0; s < Columns.Length; s++)
                {
                    var property = Entity?.GetPropertyDisplayName(Columns[s].PropertyName);
                    if (string.IsNullOrWhiteSpace(Columns[s].DisplayName))
                        Columns[s].DisplayName = property;
                }
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

        public void SetCurrentPage(ApplicationService app, CoreDbContext ctx, int currentPage = 1, SearchOptions searchOptions = null)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(SetCurrentPage) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx, currentPage, searchOptions });
        }
        public void SetCurrentPage<T>(ApplicationService app, CoreDbContext ctx, int currentPage = 1, SearchOptions searchOptions = null) where T : class
        {
            //Todo: Validate if still needed. GC.Collect();
            SearchOptions = searchOptions;
            var list = GetItemsList<T>(ctx, app);

            if (searchOptions?.HasSearch() ?? false)
            {
                var searchProperty = Array.Find(SearchProperties, x => x.PropertyName == searchOptions.SearchProperty);
                if (searchProperty != null)
                {
                    searchOptions.SearchType = searchProperty.SearchType;
                    searchOptions.ComparisonType = searchProperty.ComparisonType;
                    list = searchOptions.SearchList(list);
                }
            }

            if (!string.IsNullOrWhiteSpace(Sort))
                list = list.OrderBy(Sort);

            if (ShowPagination && PaginationOptions != null)
            {
                PaginationOptions.ChangePage(currentPage, list.Count());
                if (list.Count() > PaginationOptions.PageSize)
                    list = list.Skip((PaginationOptions.PageSize ?? 0) * ((PaginationOptions.ActualPage ?? 0) - 1)).Take(PaginationOptions.PageSize ?? 0);
            }

            CurrentObject = null;
            ItemsCollection = list;
        }

        public object GetCellValue(ApplicationService app, string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) && (Columns?.Length ?? 0) > 0)
            {
                var column = Array.Find(Columns, x => x.PropertyName == propertyName);
                if (column != null)
                    return GetCellValue(app, column);
            }
            return null;
        }
        public object GetCellValue(ApplicationService app, Column column)
        {
            return GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetCellValue) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, column, null });
        }
        public object GetCellValue<T>(ApplicationService app, Column column, string customProperty = null) where T : class
        {
            object value = (new[] { (T)CurrentObject }).AsQueryable().Select(customProperty ?? column.PropertyName).FirstOrDefault();

            if (value is Enum enumValue)
                return enumValue.GetEnumDisplayName();
            else
            {
                value = TryGetDefaultPropertyValue(app, value);

                if (!string.IsNullOrWhiteSpace(column.IsNullValue) && value == null)
                    value = column.IsNullValue;
            }
            return !string.IsNullOrWhiteSpace(column.FormatType) ? string.Format(column.FormatType, value ?? "") : value;
        }

        private object TryGetDefaultPropertyValue(ApplicationService app, object value)
        {
            if (value.GetType().ImplementsInterface<IEntity>() || value.GetType().ImplementsInterface<IQuery>())
            {
                var valueEntity = app.GetDefaultProperty(value.GetType().GetCorrectType().FullName);
                if (!string.IsNullOrWhiteSpace(valueEntity))
                    return TryGetDefaultPropertyValue(app, value.GetType().GetProperty(valueEntity).GetValue(value));
            }
            return value;
        }

        public void SetItemsCollection(ApplicationService app, CoreDbContext ctx, object entity, string collection)
        {
            SetItemsCollection(app, ctx, (IEnumerable)entity.GetType().GetProperty(collection).GetValue(entity));
        }
        public void SetItemsCollection(ApplicationService app, CoreDbContext ctx, IEnumerable collection)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(SetItemsCollection) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx, collection });
        }
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

        public int GetItemsCount(CoreDbContext ctx, ApplicationService app)
        {
            return (int)GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetItemsCount) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { ctx, app });
        }
        public int GetItemsCount<T>(CoreDbContext ctx, ApplicationService app) where T : class
        {
            return GetItemsList<T>(ctx, app).Count();
        }

        private IQueryable<T> GetItemsList<T>(CoreDbContext ctx, ApplicationService app) where T : class
        {
            IQueryable<T> list;

            if (typeof(T).ImplementsInterface<IEntity>())
            {
                list = ctx.Set<T>().AsQueryable();

                if (!string.IsNullOrEmpty(FromSql))
                    list = list.FromSql(FromSql);
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

            if (!string.IsNullOrWhiteSpace(Criteria))
                list = list.Where(app.ExpressionsService, this);

            return list;
        }

        #endregion

    }
}