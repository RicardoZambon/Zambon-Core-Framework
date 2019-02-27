using System.ComponentModel;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    public class ListViewDefaults : XmlNode
    {

        [XmlAttribute("CanEdit"), Browsable(false)]
        public string BoolCanEdit
        {
            get { return CanEdit?.ToString(); }
            set { bool.TryParse(value, out bool canEdit); CanEdit = canEdit; }
        }
        [XmlIgnore]
        public bool? CanEdit { get; set; }

        [XmlAttribute("DefaultAction")]
        public string DefaultAction { get; set; }

        [XmlAttribute("DefaultEditModalParameter")]
        public string DefaultEditModalParameter { get; set; }


        [XmlAttribute("ShowPagination"), Browsable(false)]
        public string BoolShowPagination
        {
            get { return ShowPagination?.ToString(); }
            set { bool.TryParse(value, out bool showPagination); ShowPagination = showPagination; }
        }
        [XmlIgnore]
        public bool? ShowPagination { get; set; }

        [XmlAttribute("PageSize"), Browsable(false)]
        public string IntPageSize
        {
            get { return PageSize.ToString(); }
            set { int.TryParse(value, out int pageSize); PageSize = pageSize; }
        }
        [XmlIgnore]
        public int? PageSize { get; set; }

        [XmlAttribute("PagesToShow"), Browsable(false)]
        public string IntPagesToShow
        {
            get { return PagesToShow?.ToString(); }
            set { int.TryParse(value, out int pagesToShow); PagesToShow = pagesToShow; }
        }
        [XmlIgnore]
        public int? PagesToShow { get; set; }

    }
}