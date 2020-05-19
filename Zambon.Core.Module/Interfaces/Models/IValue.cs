using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IValue : IParent, IModelValidation
    {
        string Key { get; set; }

        string DisplayName { get; set; }
    }
}