using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ISearchPropertiesParent<TSearchProperty> : IParent
        where TSearchProperty : ISearchProperty
    {
        ChildItemCollection<TSearchProperty> SearchPropertiesList { get; set; }
    }
}