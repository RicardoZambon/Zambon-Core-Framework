﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ColumnBase : SerializeNodeBase, IColumn
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute]
        public string PropertyName { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        [XmlAttribute]
        public string Size { get; set; }

        [XmlAttribute]
        public string Alignment { get; set; }

        [XmlAttribute]
        public string FormatType { get; set; }

        [XmlAttribute]
        public string FormatRegex { get; set; }

        [XmlAttribute]
        public string FormatRegexReplacement { get; set; }

        [XmlAttribute(nameof(Index)), Browsable(false)]
        public string IntIndex
        {
            get { return Index?.ToString(); }
            set { if (value != null) { int.TryParse(value, out int index); Index = index; } }
        }

        [XmlAttribute]
        public string NullValue { get; set; }

        #endregion

        #region Properties

        [XmlIgnore]
        public int? Index { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the Index with other menu object, to sort the menus array.
        /// </summary>
        /// <param name="obj">The menu object to compare to.</param>
        /// <returns>A signed number indicating the relative values of this instance and value. Return
        ///     Value Description Less than zero: This instance is less than value. Zero: This
        ///     instance is equal to value. Greater than zero: This instance is greater than value.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is ColumnBase objColumn)
            {
                return (Index ?? 0).CompareTo(objColumn.Index ?? 0);
            }
            throw new ArgumentException("Object is not a column.");
        }

        #endregion
    }
}