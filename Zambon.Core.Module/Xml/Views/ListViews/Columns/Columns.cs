using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Columns
{
    public class Columns : XmlNode
    {

        [XmlElement("Column")]
        public Column[] Column { get; set; }

    }
}