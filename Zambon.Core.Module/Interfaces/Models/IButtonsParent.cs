using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IButtonsParent<TButton> : IParent
        where TButton : IButton
    {
        ChildItemCollection<TButton> ButtonsList { get; set; }
    }
}