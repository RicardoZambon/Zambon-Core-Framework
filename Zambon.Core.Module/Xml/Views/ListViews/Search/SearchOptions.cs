using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    public class SearchOptions
    {

        public string SearchProperty { get; set; }

        /// <summary>
        /// Text (Default)
        /// Number
        /// NumberRange
        /// DateTime
        /// DateTimeRange
        /// </summary>
        public string SearchType { get; internal set; }

        /// <summary>
        /// Contains (Default)
        /// Equal
        /// GreatherThan
        /// LowerThan
        /// GreatherOrEqual
        /// LowerOrEqual
        /// StartsWith
        /// EndsWith
        /// </summary>
        public string ComparisonType { get; internal set; }


        public string SearchText { get; set; }

        public double? SearchNumber1 { get; set; }
        public double? SearchNumber2 { get; set; }

        public DateTime? SearchDateTime1 { get; set; }
        public DateTime? SearchDateTime2 { get; set; }


        #region Methods

        public bool HasSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchText) || SearchNumber1 != null || SearchDateTime1 != null;
        }

        public IQueryable<T> SearchList<T>(IQueryable<T> list) where T : class
        {
            var searchCriteria = string.Empty;
            switch (SearchType)
            {
                case "Number":
                case "DateTime":
                    searchCriteria = GetSearchNumberDateTime();
                    break;
                case "NumberRange":
                case "DateTimeRange":
                    searchCriteria = "{0} >= @0 && {0} <= @1";
                    break;
                default:
                    searchCriteria = GetSearchText();
                    break;
            }
            searchCriteria = string.Format(searchCriteria, SearchProperty);

            switch (SearchType)
            {
                case "Number":
                    return list.Where(searchCriteria, SearchNumber1);
                case "NumberRange":
                    return list.Where(searchCriteria, SearchNumber1, SearchNumber2);
                case "DateTime":
                    return list.Where(searchCriteria, SearchDateTime1);
                case "DateTimeRange":
                    return list.Where(searchCriteria, SearchDateTime1, SearchDateTime2);
                default:
                    return list.Where(searchCriteria, SearchText);
            }
        }

        private string GetSearchText()
        {
            switch (ComparisonType)
            {
                case "Equal":
                    return "{0} != null && {0}.ToString().ToLower().Equals(@0)";
                case "StartsWith":
                    return "{0} != null && {0}.ToString().ToLower().StartsWith(@0)";
                case "EndsWith":
                    return "{0} != null && {0}.ToString().ToLower().EndsWith(@0)";
                default:
                    return "{0} != null && {0}.ToString().ToLower().Contains(@0)";
            }
        }
        private string GetSearchNumberDateTime()
        {
            switch (ComparisonType)
            {
                case "GreatherThan":
                    return "{0} > @0";
                case "LowerThan":
                    return "{0} < @0";
                case "GreatherOrEqual":
                    return "{0} >= @0";
                case "LowerOrEqual":
                    return "{0} <= @0";
                default:
                    return "{0} == @0";
            }
        }
        

        #endregion
    }
}