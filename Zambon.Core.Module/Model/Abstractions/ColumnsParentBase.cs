using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ColumnsParentBase<TColumn> : SerializeNodeBase, IColumnsParent<TColumn>
        where TColumn : ColumnBase
    {
        #region Constants

        private const string COLUMNS_NODE = "Column";

        #endregion

        #region XML Elements

        [XmlElement(COLUMNS_NODE)]
        public ChildItemCollection<TColumn> ColumnsList { get; set; }

        #endregion

        #region Constructors

        public ColumnsParentBase()
        {
            ColumnsList = new ChildItemCollection<TColumn>(this);
        }

        #endregion
    }
}