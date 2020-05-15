using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IProperties<TProperty> : IParent where TProperty : IProperty
    {
        ChildItemCollection<TProperty> PropertiesList { get; set; }
    }
}