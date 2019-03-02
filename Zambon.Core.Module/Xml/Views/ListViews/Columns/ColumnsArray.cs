using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews.Columns
{
    /// <summary>
    /// Represents a list of columns used in ListViews or LookUpViews.
    /// </summary>
    public class ColumnsArray : XmlNode
    {

        /// <summary>
        /// List of all columns.
        /// </summary>
        [XmlElement("Column")]
        public Column[] Column { get; set; }

    }
}