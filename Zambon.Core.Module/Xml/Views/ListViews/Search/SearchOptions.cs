using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    /// <summary>
    /// Represents a search instance used by ListViews/LookUpViews.
    /// </summary>
    public class SearchOptions
    {
        /// <summary>
        /// The property to search for.
        /// </summary>
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


        /// <summary>
        /// When the property is a string, the text to search for.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// When the property is an integer, the number to search for, when ranged search the beginning number.
        /// </summary>
        public double? SearchNumber1 { get; set; }
        /// <summary>
        /// When the property is an integer, the number to search for, when ranged search the ending number
        /// </summary>
        public double? SearchNumber2 { get; set; }

        /// <summary>
        /// When the property is a date/time, the date to search for, when ranged search the beginning date.
        /// </summary>
        public DateTime? SearchDateTime1 { get; set; }
        /// <summary>
        /// When the property is a date/time, the date to search for, when ranged search the ending date.
        /// </summary>
        public DateTime? SearchDateTime2 { get; set; }


        #region Methods

        /// <summary>
        /// Checks if were informed in any search type.
        /// </summary>
        /// <returns>Returns true when should execute the search.</returns>
        public bool HasSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchText) || SearchNumber1 != null || SearchDateTime1 != null;
        }

        /// <summary>
        /// Executes the search in a IQueryable list.
        /// </summary>
        /// <typeparam name="T">The list element type.</typeparam>
        /// <param name="list">The IQueryable list.</param>
        /// <returns>Returns the IQueryable list already searched.</returns>
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