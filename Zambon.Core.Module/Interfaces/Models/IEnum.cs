using Zambon.Core.Module.Interfaces.Models.Validations;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEnum : IParent, IModelValidation
    {
        string Id { get; set; }
    }

    public interface IEnum<TValue> : IEnum
        where TValue : IValue
    {
        ChildItemCollection<TValue> Values { get; set; }
    }
}