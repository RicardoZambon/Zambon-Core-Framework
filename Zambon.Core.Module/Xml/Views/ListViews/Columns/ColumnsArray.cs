using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Columns
{
    public class ColumnsArray : XmlNode
    {

        [XmlElement("Column")]
        public Column[] Column { get; set; }

    }
}