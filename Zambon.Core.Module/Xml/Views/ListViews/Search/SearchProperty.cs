using Zambon.Core.Database;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    public class SearchProperty : XmlNode, IComparable
    {

        [XmlAttribute("PropertyName"), MergeKey]
        public string PropertyName { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("Type")]
        public string SearchType { get; set; }

        [XmlAttribute("SearchType")]
        public string ComparisonType { get; set; }

        [XmlAttribute("DefaultValue")]
        public string DefaultValue { get; set; }

        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            base.OnLoading(app, ctx);

            if (SearchType == null)
                SearchType = "Text";

            if (ComparisonType == null)
                if (SearchType == "Text")
                    ComparisonType = "Contains";
                else
                    ComparisonType = "Equal";
        }

        #endregion

        #region Methods

        public int CompareTo(object obj)
        {
            if (obj is SearchProperty)
                return Index.CompareTo(((SearchProperty)obj).Index);
            throw new ArgumentException("Object is not a search property.");
        }

        #endregion

    }
}