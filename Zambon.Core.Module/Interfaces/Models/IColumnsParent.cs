using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IColumnsParent<TColumn> : IParent
        where TColumn : IColumn
    {
        ChildItemCollection<TColumn> ColumnsList { get; set; }
    }
}