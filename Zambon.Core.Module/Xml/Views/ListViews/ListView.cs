using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Helper;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Configuration;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.ListViews.Paint;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Xml.Views.SubViews;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Database.ExtensionMethods;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    public class ListView : View
    {

        [XmlAttribute("CanEdit")]
        public string BoolCanEdit { get; set; }
        [XmlIgnore]
        public bool CanEdit { get { return bool.Parse(BoolCanEdit?.ToLower() ?? "false"); } }

        [XmlAttribute("EditModalId")]
        public string EditModalId { get; set; }

        [XmlAttribute("EditModalParameters")]
        public string EditModalParameters { get; set; }


        [XmlIgnore]
        public DetailModal EditModal { get; private set; }

        [XmlAttribute("Sort")]
        public string Sort { get; set; }

        [XmlAttribute("Criteria")]
        public string Criteria { get; set; }

        [XmlAttribute("CriteriaArguments")]
        public string CriteriaArguments { get; set; }

        [XmlAttribute("MessageOnEmpty")]
        public string MessageOnEmpty { get; set; }

        [XmlElement("SearchProperties")]
        public SearchProperties SearchProperties { get; set; }

        [XmlElement("Columns")]
        public Columns.Columns Columns { get; set; }

        [XmlElement("PaintOptions")]
        public PaintOptions PaintOptions { get; set; }

        [XmlAttribute("ShowPagination")]
        public string BoolShowPagination { get; set; }
        [XmlIgnore]
        public bool ShowPagination { get { return bool.Parse(BoolShowPagination?.ToLower() ?? "false"); } }

        [XmlElement("PaginationOptions")]
        public PaginationOptions PaginationOptions { get; set; }


        [XmlIgnore]
        public IDictionary<string, object> CustomParameters { get; private set; } = new Dictionary<string, object>();

        #region Overrides

        internal override void OnLoading(Application app, CoreDbContext ctx)
        {
            base.OnLoading(app, ctx);

            if (string.IsNullOrWhiteSpace(ControllerName))
                ControllerName = Entity.DefaultController;

            if (string.IsNullOrWhiteSpace(ActionName))
                ActionName = app.ModuleConfiguration.ListViewDefaults.DefaultAction;

            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
            {
                for (var d = 0; d < SubViews.DetailViews.Length; d++)
                {
                    SubViews.DetailViews[d].ParentViewId = ViewId;
                    SubViews.DetailViews[d].LoadView(app);
                }

                if (string.IsNullOrWhiteSpace(EditModalId))
                    EditModalId = Array.Find(SubViews.DetailViews, x => x.View.Type == Type)?.Id;

                if (string.IsNullOrWhiteSpace(EditModalParameters))
                    EditModalParameters = "objectId=[ID]";

                EditModal = Array.Find(SubViews.DetailViews, x => x.Id == EditModalId);
            }

            if ((SubViews?.LookupViews?.Length ?? 0) > 0)
                for (var l = 0; l < SubViews.LookupViews.Length; l++)
                {
                    SubViews.LookupViews[l].ParentViewId = ViewId;
                    SubViews.LookupViews[l].LoadView(app);
                }

            if ((SubViews?.SubListViews?.Length ?? 0) > 0)
                for (var s = 0; s < SubViews.SubListViews.Length; s++)
                {
                    SubViews.SubListViews[s].ParentViewId = ViewId;
                    SubViews.SubListViews[s].LoadView(app);
                }

            if (string.IsNullOrWhiteSpace(BoolCanEdit))
                BoolCanEdit = app.ModuleConfiguration?.ListViewDefaults?.BoolCanEdit;

            if (string.IsNullOrWhiteSpace(BoolShowPagination))
                BoolShowPagination = app.ModuleConfiguration?.ListViewDefaults?.BoolShowPagination;

            if ((SearchProperties?.SearchProperty?.Length ?? 0) > 0)
            {
                Array.Sort(SearchProperties.SearchProperty);
                for (var s = 0; s < SearchProperties.SearchProperty.Length; s++)
                {
                    var property = Entity?.GetProperty(SearchProperties.SearchProperty[s].PropertyName);
                    if (string.IsNullOrWhiteSpace(SearchProperties.SearchProperty[s].DisplayName))
                        SearchProperties.SearchProperty[s].DisplayName = property?.DisplayName;
                    SearchProperties.SearchProperty[s].OnLoading(app, ctx);
                }
            }

            if ((Buttons?.Button?.Length ?? 0) > 0)
            {
                for (var b = 0; b < Buttons.Button.Length; b++)
                    OnLoadingSubButtons(Buttons.Button[b]);
            }

            if ((Columns?.Column?.Length ?? 0) > 0)
            {
                Array.Sort(Columns.Column);
                for (var s = 0; s < Columns.Column.Length; s++)
                {
                    var property = Entity?.GetProperty(Columns.Column[s].PropertyName);
                    if (string.IsNullOrWhiteSpace(Columns.Column[s].DisplayName))
                        Columns.Column[s].DisplayName = property?.DisplayName;
                }
            }

            if (PaginationOptions == null)
                PaginationOptions = new PaginationOptions();

            if (PaginationOptions.PageSize == 0)
                PaginationOptions.PageSize = app.ModuleConfiguration.ListViewDefaults.PageSize;

            if (PaginationOptions.PagesToShow == 0)
                PaginationOptions.PagesToShow = app.ModuleConfiguration.ListViewDefaults.PagesToShow;
        }

        private void OnLoadingSubButtons(Buttons.Button button)
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

            if ((button.SubButtons?.Button?.Length ?? 0) > 0)
                for (var b = 0; b < button.SubButtons.Button.Length; b++)
                    OnLoadingSubButtons(button.SubButtons.Button[b]);
        }

        #endregion

        #region Methods

        public DetailModal GetDetailModal(string Id)
        {
            if ((SubViews?.DetailViews?.Length ?? 0) > 0)
                return Array.Find(SubViews.DetailViews, modal => modal.Id == Id);
            return null;
        }

        public void SetCurrentPage(ApplicationService _app, CoreDbContext _ctx, int _page = 1, SearchOptions searchOptions = null)
        {
            typeof(ListView).GetMethods().FirstOrDefault(x => x.Name == "SetCurrentPage" && x.GetGenericArguments().Count() == 1).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, _ctx, _page, searchOptions });
        }
        public void SetCurrentPage<T>(ApplicationService _app, CoreDbContext _ctx, int _page = 1, SearchOptions searchOptions = null) where T : class
        {
            GC.Collect();

            _app.SetListViewSearchOptions(ViewId, searchOptions);

            IQueryable<T> list;
            if (typeof(T).ImplementsInterface<IEntity>())
                list = _ctx.Set<T>().AsQueryable();
            else if (typeof(T).ImplementsInterface<IQuery>())
                list = _ctx.Query<T>();
            else
                throw new ApplicationException($"The ClrType informed in EntityType \"{Type}\" is does not implement IEntity or IQuery interfaces.");

            if (!string.IsNullOrEmpty(Entity.FromSql))
                list = list.FromSql(Entity.FromSql);

            if (!string.IsNullOrWhiteSpace(Criteria))
                list = list.Where(Criteria, _app.Expressions.FormatExpressionValues(CriteriaArguments.Split(',')));

            if (searchOptions?.HasSearch() ?? false)
            {
                var searchProperty = Array.Find(SearchProperties.SearchProperty, x => x.PropertyName == searchOptions.SearchProperty);
                if (searchProperty != null)
                {
                    searchOptions.SearchType = searchProperty.SearchType;
                    searchOptions.ComparisonType = searchProperty.ComparisonType;
                    list = searchOptions.SearchList(list);
                }
            }

            var sort = Sort;
            if (string.IsNullOrWhiteSpace(Sort))
                sort = typeof(T).GetDefaultProperty();

            if (!string.IsNullOrWhiteSpace(sort))
                list = list.OrderBy(sort);

            if (ShowPagination && PaginationOptions != null)
            {
                PaginationOptions.ChangePage(_page, list.Count());

                if (list.Count() > PaginationOptions.PageSize)
                    list = list.Skip(PaginationOptions.PageSize * (PaginationOptions.ActualPage - 1)).Take(PaginationOptions.PageSize);
            }

            _app.ClearListViewCurrentObject(ViewId);
            _app.SetListViewItemsCollection(ViewId, list);
        }


        public object GetCellValue(ApplicationService _app, Column column)
        {
            return typeof(ListView).GetMethod("GetTypedCellValue").MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, column.PropertyName, column.FormatType, column.IsNullValue });
        }
        public object GetCellValue(ApplicationService _app, string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                if ((Columns?.Column?.Length ?? 0) > 0)
                {
                    var column = Array.Find(Columns.Column, x => x.PropertyName == propertyName);
                    if (column != null)
                        return GetCellValue(_app, column);
                }
                return typeof(ListView).GetMethod("GetTypedCellValue").MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, propertyName, "", "" });
            }
            return null;
        }
        public object GetTypedCellValue<T>(ApplicationService _app, string propertyName, string formatType = "", string isNullValue = "") where T : class
        {
            var value = GetObjectValue((new[] { (T)_app.GetListViewCurrentObject(ViewId) }).AsQueryable(), propertyName);

            if (value is Enum)
                return EnumExtension.GetEnumDisplayName(value);
            else if (value is BaseDBObject valueDBObject)
            {
                var defaultProperty = valueDBObject.GetType().GetDefaultProperty();
                if (string.IsNullOrWhiteSpace(defaultProperty))
                    defaultProperty = "ID";

                return GetTypedCellValue<T>(_app, propertyName + "." + defaultProperty, formatType, isNullValue);
            }
            else
            {
                if (value is string && value == null && !string.IsNullOrWhiteSpace(isNullValue))
                    value = isNullValue;
                return !string.IsNullOrWhiteSpace(formatType) ? string.Format(formatType, value ?? "") : value;
            }
        }

        private object GetObjectValue(IQueryable query, string propertyName)
        {
            var properties = propertyName.Split(".");
            var property = properties[0];

            query = query.Where(property + " != null");

            var value = query.Select(property).FirstOrDefault();
            if (value != null)
            {
                if (properties.Length > 1)
                    return GetObjectValue(query.Select( property, null), string.Join(".", properties, 1, properties.Length - 1));
                else
                    return value;
            }
            return null;
        }

        public void SetItemsCollection(BaseDBObject entity, ApplicationService _app, string collection)
        {
            typeof(ListView).GetMethods().FirstOrDefault(x => x.Name == "SetItemsCollection" && x.GetGenericArguments().Count() == 1).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, entity.GetType().GetProperty(collection).GetValue(entity) });
        }
        public void SetItemsCollection<T>(ApplicationService _app, ICollection<T> collection) where T : class
        {
            IQueryable<T> list = null;
            if (collection != null)
            {
                list = collection.AsQueryable();

                if (typeof(T).IsSubclassOf(typeof(DBObject)))
                {
                    //var param = System.Linq.Expressions.Expression.Parameter(typeof(T));
                    //var exp = System.Linq.Expressions.Expression.Equal(System.Linq.Expressions.Expression.Property(param, "IsDeleted"), System.Linq.Expressions.Expression.Constant(false));
                    //var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(exp, param);
                    //list = list.Where(lambda);

                    list = list.Where("!IsDeleted");
                }

                if (!string.IsNullOrWhiteSpace(Criteria))
                    list = list.Where(Criteria, _app.Expressions.FormatExpressionValues(CriteriaArguments?.Split(',')) ?? new string[0]);

                var sort = Sort;
                if (string.IsNullOrWhiteSpace(Sort))
                    sort = typeof(T).GetDefaultProperty();

                if (!string.IsNullOrWhiteSpace(sort))
                    list = list.OrderBy(sort);
            }
            _app.ClearListViewCurrentObject(ViewId);
            _app.SetListViewItemsCollection(ViewId, list);
        }

        public int GetItemsCount(CoreDbContext _ctx, ApplicationService _app)
        {
            return (int)typeof(ListView).GetMethods().FirstOrDefault(x => x.Name == "GetItemsCount" && x.GetGenericArguments().Count() == 1).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _ctx, _app });
        }
        public int GetItemsCount<T>(CoreDbContext _ctx, ApplicationService _app) where T : class
        {
            var list = _ctx.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Criteria))
                list = list.Where(Criteria, _app.Expressions.FormatExpressionValues(CriteriaArguments.Split(',')));

            return list.Count();
        }


        public void SetParameter(string key, object value)
        {
            if (CustomParameters.ContainsKey(key))
                CustomParameters[key] = value;
            else
                CustomParameters.Add(key, value);
        }

        public void ClearParameters()
        {
            CustomParameters.Clear();
        }

        #endregion

    }
}