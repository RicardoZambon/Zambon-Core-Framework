using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEnum<TValue> : IParent
        where TValue : IValue
    {
        string Id { get; set; }

        ChildItemCollection<TValue> ValuesList { get; set; }
    }
}