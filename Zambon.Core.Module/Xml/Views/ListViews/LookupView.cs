using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Expressions;
using Zambon.Core.Module.Helper;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    public class LookupView : BaseView
    {

        [XmlAttribute("Sort")]
        public string Sort { get; set; }

        [XmlAttribute("Criteria")]
        public string Criteria { get; set; }

        [XmlAttribute("CriteriaArguments")]
        public string CriteriaArguments { get; set; }

        [XmlElement("SearchProperties")]
        public SearchProperties SearchProperties { get; set; }

        [XmlElement("Columns")]
        public Columns.Columns Columns { get; set; }

        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            base.OnLoading(app, ctx);

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
        }

        #endregion

        #region Methods

        public void PopulateView(ApplicationService _app, CoreContext _ctx, SearchOptions searchOptions = null)
        {
            typeof(LookupView).GetMethods().FirstOrDefault(x => x.Name == "PopulateView" && x.GetGenericArguments().Count() == 1).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, _ctx, searchOptions });
        }
        public void PopulateView<T>(ApplicationService _app, CoreContext _ctx, SearchOptions searchOptions = null) where T : BaseDBObject
        {
            GC.Collect();
            _app.SetLookUpViewSearchOptions(ViewId, searchOptions);
            var list = _ctx.Set<T>().AsQueryable();

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

            _app.ClearLookUpViewCurrentObject(ViewId);
            _app.SetLookUpViewItemsCollection(ViewId, list);
        }

        public object GetCellValue(ApplicationService _app, Column column)
        {
            return typeof(LookupView).GetMethod("GetTypedCellValue").MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, column.PropertyName, column.FormatType, column.IsNullValue });
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
                return typeof(LookupView).GetMethod("GetTypedCellValue").MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { _app, propertyName, "", "" });
            }
            return null;
        }
        public object GetTypedCellValue<T>(ApplicationService _app, string propertyName, string formatType = "", string isNullValue = "") where T : BaseDBObject
        {
            var value = (new[] { (T)_app.GetLookUpViewCurrentObject(ViewId) }).AsQueryable().Select(propertyName).FirstOrDefault();
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

        #endregion

    }
}