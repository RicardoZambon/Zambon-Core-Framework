using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEnumsParent<TEnum, TValue> : IParent
        where TEnum : IEnum<TValue>
            where TValue : IValue
    {
        ChildItemCollection<TEnum> EnumsList { get; set; }
    }
}