using Microsoft.EntityFrameworkCore;
using System;
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
using Zambon.Core.Module.Xml.Views.ListViews.Columns;
using Zambon.Core.Module.Xml.Views.ListViews.Search;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    public class LookupView : BaseView, ICriteria
    {

        [XmlAttribute("Sort")]
        public string Sort { get; set; }

        [XmlAttribute("FromSql")]
        public string FromSql { get; set; }

        [XmlAttribute("Criteria")]
        public string Criteria { get; set; }

        [XmlAttribute("CriteriaArguments")]
        public string CriteriaArguments { get; set; }

        [XmlIgnore]
        public SearchProperty[] SearchProperties { get { return _SearchProperties?.SearchProperty; } }

        [XmlIgnore]
        public Column[] Columns { get { return _Columns?.Column; } }


        [XmlElement("SearchProperties"), Browsable(false)]
        public SearchPropertiesArray _SearchProperties { get; set; }

        [XmlElement("Columns"), Browsable(false)]
        public ColumnsArray _Columns { get; set; }

        
        [XmlIgnore]
        public SearchOptions SearchOptions { get; private set; }

        [XmlIgnore]
        public object CurrentObject { get; set; }

        [XmlIgnore]
        public IQueryable ItemsCollection { get; set; }

        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (string.IsNullOrWhiteSpace(Sort))
                Sort = Entity.GetDefaultProperty();

            if (string.IsNullOrWhiteSpace(FromSql) && !string.IsNullOrWhiteSpace(Entity.FromSql))
                FromSql = Entity.FromSql;

            if ((SearchProperties?.Length ?? 0) > 0)
            {
                Array.Sort(SearchProperties);
                for (var s = 0; s < SearchProperties.Length; s++)
                    if (string.IsNullOrWhiteSpace(SearchProperties[s].DisplayName))
                        SearchProperties[s].DisplayName = Entity?.GetPropertyDisplayName(SearchProperties[s].PropertyName); ;
            }

            if ((Columns?.Length ?? 0) > 0)
            {
                Array.Sort(Columns);
                for (var s = 0; s < Columns.Length; s++)
                    if (string.IsNullOrWhiteSpace(Columns[s].DisplayName))
                        Columns[s].DisplayName = Entity?.GetPropertyDisplayName(Columns[s].PropertyName);
            }
        }

        #endregion

        #region Methods

        public void PopulateView(ApplicationService app, CoreDbContext ctx, SearchOptions searchOptions = null)
        {
            GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(PopulateView) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, ctx, searchOptions });
        }
        public void PopulateView<T>(ApplicationService app, CoreDbContext ctx, SearchOptions searchOptions = null) where T : class
        {
            //Todo: Validate if still needed. GC.Collect();
            SearchOptions = searchOptions;

            var list = GetItemsList<T>(ctx);

            if (!string.IsNullOrWhiteSpace(Criteria))
                list = list.Where(app.Expressions, this);
            
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
            return GetType().GetMethods().FirstOrDefault(x => x.Name == nameof(GetCellValue) && x.IsGenericMethod).MakeGenericMethod(Entity.GetEntityType()).Invoke(this, new object[] { app, column });
        }
        public object GetCellValue<T>(ApplicationService app, Column column, string customProperty = null) where T : class
        {
            object value = (new[] { (T)CurrentObject }).AsQueryable().Select(customProperty ?? column.PropertyName).FirstOrDefault();

            if (value is Enum enumValue)
                return enumValue.GetEnumDisplayName();

            if (value.GetType().ImplementsInterface<IEntity>() || value.GetType().ImplementsInterface<IQuery>())
            {
                var valueEntity = app.GetDefaultProperty(value.GetType().FullName);
                if (!string.IsNullOrWhiteSpace(valueEntity))
                    value = valueEntity.GetType().GetProperty(valueEntity).GetValue(value);
            }

            if (!string.IsNullOrWhiteSpace(column.IsNullValue) && value == null)
                value = column.IsNullValue;

            return !string.IsNullOrWhiteSpace(column.FormatType) ? string.Format(column.FormatType, value ?? "") : value;
        }
        
        private IQueryable<T> GetItemsList<T>(CoreDbContext ctx) where T : class
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
                    throw new ApplicationException($"The LookupView \"{ViewId}\" has an entity \"{Entity}\" that implements the IQuery interface and is mandatory inform the attribute FromSql in LookupView or Entity definition.");
            }
            else
                throw new ApplicationException($"The LookupView \"{ViewId}\" entity \"{Entity}\" does not have implemented the interface IEntity not IQuery.");
            return list;
        }

        #endregion

    }
}