using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IDetailView : IView
    {
    }

    public interface IDetailView<TButton> : IDetailView, IViewButtons<TButton>
        where TButton : IButton
    {
    }
}