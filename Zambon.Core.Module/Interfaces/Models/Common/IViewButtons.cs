using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models.Common
{
    public interface IViewButtons<TButton>
        where TButton : IButton<TButton>
    {
        ChildItemCollection<TButton> Buttons { get; set; }
    }
}