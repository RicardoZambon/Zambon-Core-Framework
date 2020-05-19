using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ISearchProperty : IParent, IModelValidation
    {
        string PropertyName { get; set; }

        string DisplayName { get; set; }
    }
}