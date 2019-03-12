using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Database;

namespace Zambon.Core.Module.Xml.Views.ListViews.Search
{
    /// <summary>
    /// Represents a node <SearchProperty /> from XML Application Model.
    /// </summary>
    public class SearchProperty : XmlNode, IComparable
    {

        /// <summary>
        /// The PropertyName attribute from XML. The property name to use when executing the search.
        /// </summary>
        [XmlAttribute("PropertyName"), MergeKey]
        public string PropertyName { get; set; }

        /// <summary>
        /// The DisplayName attribute from XML. The display name in selection field, by default will use the display name from the property model.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The Index attribute from XML. The index value returned from Xml file.
        /// </summary>
        [XmlAttribute("Index"), Browsable(false)]
        public string IntIndex
        {
            get { return Index.ToString(); }
            set { if (value != null) { int.TryParse(value, out int index); Index = index; } }
        }
        /// <summary>
        /// The Index attribute from XML. The column index order to display.
        /// </summary>
        [XmlIgnore]
        public int? Index { get; set; }

        /// <summary>
        /// The Type attribute from XML. The type of search should be executed.
        /// </summary>
        [XmlAttribute("Type")]
        public string Type { get; set; }

        /// <summary>
        /// The Comparison attribute from XML. The comparison type of search should be executed.
        /// </summary>
        [XmlAttribute("Comparison")]
        public string Comparison { get; set; }

        /// <summary>
        /// The DefaultValue attribute from XML. If should use a default value when selecting this search option.
        /// </summary>
        [XmlAttribute("DefaultValue")]
        public string DefaultValue { get; set; }

        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            base.OnLoadingXml(app, ctx);

            if (Type == null)
                Type = "Text";

            if (Comparison == null)
                if (Type == "Text")
                    Comparison = "Contains";
                else
                    Comparison = "Equal";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares the Index with other SearchProperty object, to sort the SearchProperties array.
        /// </summary>
        /// <param name="obj">The SearchProperty object to compare to.</param>
        /// <returns>A signed number indicating the relative values of this instance and value. Return
        ///     Value Description Less than zero: This instance is less than value. Zero: This
        ///     instance is equal to value. Greater than zero: This instance is greater than value.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is SearchProperty objSearchProperty)
                return (Index ?? 0).CompareTo(objSearchProperty.Index ?? 0);
            throw new ArgumentException("Object is not a search property.");
        }

        #endregion

    }
}