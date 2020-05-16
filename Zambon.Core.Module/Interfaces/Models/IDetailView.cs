using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IDetailView<TButtonsParent, TButton> : IView, IViewButtons<TButtonsParent, TButton>
        where TButtonsParent : IButtonsParent<TButton>
            where TButton : IButton
    {
    }
}