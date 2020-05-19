using Zambon.Core.Module.Interfaces.Models.Common;
using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IGridTemplate : IParent, ICondition, IModelValidation
    {
        string Id { get; set; }

        string ColumnIds { get; set; }
    }
}