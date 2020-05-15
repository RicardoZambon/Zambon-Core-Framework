using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IGridTemplate : IParent, ICondition
    {
        string Id { get; set; }

        string ColumnIds { get; set; }
    }
}