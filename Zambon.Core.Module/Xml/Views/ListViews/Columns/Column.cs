using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Columns
{
    public class Column : XmlNode, IComparable
    {

        [XmlAttribute("PropertyName"), MergeKey]
        public string PropertyName { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("FormatType")]
        public string FormatType { get; set; }

        [XmlAttribute("IsNullValue")]
        public string IsNullValue { get; set; }

        [XmlAttribute("Size")]
        public string Size { get; set; }

        [XmlAttribute("RecordClass")]
        public string RecordClass { get; set; }

        #region Methods

        public int CompareTo(object obj)
        {
            if (obj is Column)
                return Index.CompareTo(((Column)obj).Index);
            throw new ArgumentException("Object is not a column.");
        }

        #endregion

    }
}