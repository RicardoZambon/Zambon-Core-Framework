using System.ComponentModel;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    /// <summary>
    /// Represents a node <ListViewDefaults /> from XML Application Model. Define default values for all ListViews.
    /// </summary>
    public class ListViewDefaults : XmlNode
    {

        /// <summary>
        /// The CanEdit attribute from XML. Indicates if the list views should be editable or not.
        /// </summary>
        [XmlAttribute("CanEdit"), Browsable(false)]
        public string BoolCanEdit
        {
            get { return CanEdit?.ToString(); }
            set { bool.TryParse(value, out bool canEdit); CanEdit = canEdit; }
        }
        /// <summary>
        /// The CanEdit attribute from XML. Indicates if the list views should be editable or not.
        /// </summary>
        [XmlIgnore]
        public bool? CanEdit { get; set; }

        /// <summary>
        /// The DefaultAction attribute from XML. The default action should be used when editing ListViews.
        /// </summary>
        [XmlAttribute("DefaultAction")]
        public string DefaultAction { get; set; }

        /// <summary>
        /// The DefaultEditModalParameter attribute from XML. The default paramters should be passed to the modal when editing ListViews.
        /// </summary>
        [XmlAttribute("DefaultEditModalParameter")]
        public string DefaultEditModalParameter { get; set; }


        /// <summary>
        /// The ShowPagination attribute from XML. Indicates if should display or not a pagiation.
        /// </summary>
        [XmlAttribute("ShowPagination"), Browsable(false)]
        public string BoolShowPagination
        {
            get { return ShowPagination?.ToString(); }
            set { bool.TryParse(value, out bool showPagination); ShowPagination = showPagination; }
        }
        /// <summary>
        /// The ShowPagination attribute from XML. Indicates if should display or not a pagiation.
        /// </summary>
        [XmlIgnore]
        public bool? ShowPagination { get; set; }

        /// <summary>
        /// The PageSize attribute from XML. The maximum number of records to display in a same page.
        /// </summary>
        [XmlAttribute("PageSize"), Browsable(false)]
        public string IntPageSize
        {
            get { return PageSize.ToString(); }
            set { int.TryParse(value, out int pageSize); PageSize = pageSize; }
        }
        /// <summary>
        /// The PageSize attribute from XML. The maximum number of records to display in a same page.
        /// </summary>
        [XmlIgnore]
        public int? PageSize { get; set; }

        /// <summary>
        /// The PagesToShow attribute from XML. The maximum number of pages to show in footer.
        /// </summary>
        [XmlAttribute("PagesToShow"), Browsable(false)]
        public string IntPagesToShow
        {
            get { return PagesToShow?.ToString(); }
            set { int.TryParse(value, out int pagesToShow); PagesToShow = pagesToShow; }
        }
        /// <summary>
        /// The PagesToShow attribute from XML. The maximum number of pages to show in footer.
        /// </summary>
        [XmlIgnore]
        public int? PagesToShow { get; set; }

    }
}