using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IGridTemplatesParent<TGridTemplate> : IParent
        where TGridTemplate : IGridTemplate
    {
        ChildItemCollection<TGridTemplate> GridTemplatesList { get; set; }
    }
}