using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models.Common
{
    public interface IViewButtons<TButtonsParent, TButton>
        where TButtonsParent : IButtonsParent<TButton>
            where TButton : IButton
    {
        TButtonsParent _Buttons { get; set; }

        ChildItemCollection<TButton> Buttons { get; }
    }
}