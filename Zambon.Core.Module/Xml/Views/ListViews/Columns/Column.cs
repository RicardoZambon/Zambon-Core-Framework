
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.ListViews.Columns
{
    public class Column : XmlNode, IComparable
    {

        [XmlAttribute("PropertyName"), MergeKey]
        public string PropertyName { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { int.TryParse(value, out int index); Index = index; }
        }
        [XmlIgnore]
        public int? Index { get; set; }

        [XmlAttribute("Size")]
        public string Size { get; set; }

        [XmlAttribute("FormatType")]
        public string FormatType { get; set; }

        [XmlAttribute("Alignment")]
        public string Alignment { get; set; }

        [XmlAttribute("IsNullValue")]
        public string IsNullValue { get; set; }

        [XmlAttribute("CssClass")]
        public string CssClass { get; set; }


        #region Methods

        public int CompareTo(object obj)
        {
            if (obj is Column objColumn)
                return (Index ?? 0).CompareTo(objColumn.Index ?? 0);
            throw new ArgumentException("Object is not a column.");
        }

        #endregion

    }
}