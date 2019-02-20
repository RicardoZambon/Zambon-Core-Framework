
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Configuration
{
    public class ListViewDefaults : XmlNode
    {

        [XmlAttribute("CanEdit")]
        public string BoolCanEdit
        {
            get { return CanEdit.ToString(); }
            set { CanEdit = bool.Parse(value.ToLower()); }
        }
        [XmlIgnore]
        public bool CanEdit { get; set; }

        [XmlAttribute("DefaultAction")]
        public string DefaultAction { get; set; }

        [XmlAttribute("ShowPagination")]
        public string BoolShowPagination
        {
            get { return ShowPagination.ToString(); }
            set { ShowPagination = bool.Parse(value.ToLower()); }
        }
        [XmlIgnore]
        public bool ShowPagination { get; set; }


        [XmlAttribute("PageSize")]
        public int PageSize { get; set; }

        [XmlAttribute("PagesToShow")]
        public int PagesToShow { get; set; }

    }
}