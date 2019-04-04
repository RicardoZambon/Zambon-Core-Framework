
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.ListViews.Columns
{
    /// <summary>
    /// Represents columns used in ListViews and LookUpViews.
    /// </summary>
    public class Column : XmlNode, IComparable
    {
        /// <summary>
        /// The property name this column should show, ex: "Name" or "Role.Name".
        /// </summary>
        [XmlAttribute("PropertyName"), MergeKey]
        public string PropertyName { get; set; }

        /// <summary>
        /// The column header display name, by default will take from EntityType Property.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The index value returned from XML file.
        /// </summary>
        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { if (value != null) { int.TryParse(value, out int index); Index = index; } }
        }
        /// <summary>
        /// The column index order to display.
        /// </summary>
        [XmlIgnore]
        public int? Index { get; set; }

        /// <summary>
        /// The size of the column, by default will take the available space equally, input numbers from 1 to 12 or "fit" to set to the minimum width possible without truncating the text. The sum of all columns can not be greater than 12.
        /// </summary>
        [XmlAttribute("Size")]
        public string Size { get; set; }

        /// <summary>
        /// The format the value should be formated in string.Format pattern: {0:MM/dd/yyyy}.
        /// </summary>
        [XmlAttribute("FormatType")]
        public string FormatType { get; set; }

        /// <summary>
        /// The alignment of the cell, by default is "Left". Can set "Left", "Center" or "Right".
        /// </summary>
        [XmlAttribute("Alignment")]
        public string Alignment { get; set; }
        
        /// <summary>
        /// Value to show when the column value is null.
        /// </summary>
        [XmlAttribute("IsNullValue")]
        public string IsNullValue { get; set; }

        /// <summary>
        /// Custom CSS class to use with the cell.
        /// </summary>
        [XmlAttribute("CssClass")]
        public string CssClass { get; set; }


        #region Methods

        /// <summary>
        /// Compares the Index with other column object, to sort the columns array.
        /// </summary>
        /// <param name="obj">The column object to compare to.</param>
        /// <returns>A signed number indicating the relative values of this instance and value. Return
        ///     Value Description Less than zero: This instance is less than value. Zero: This
        ///     instance is equal to value. Greater than zero: This instance is greater than value.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is Column objColumn)
                return (Index ?? 0).CompareTo(objColumn.Index ?? 0);
            throw new ArgumentException("Object is not a column.");
        }

        #endregion
    }
}