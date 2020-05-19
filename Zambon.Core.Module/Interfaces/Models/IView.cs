using Zambon.Core.Module.Interfaces.Models.Validations;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IView : IParent, IModelValidation
    {
        string Id { get; set; }

        string Title { get; set; }

        string EntityId { get; set; }
    }
}