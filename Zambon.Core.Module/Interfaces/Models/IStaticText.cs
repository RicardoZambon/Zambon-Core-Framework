using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IStaticText : IParent, IModelValidation
    {
        string Key { get; set; }

        string Value { get; set; }
    }
}