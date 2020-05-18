using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebDetailView<TButtonsParent, TButton> : IDetailView<TButtonsParent, TButton>
        where TButtonsParent : IButtonsParent<TButton>
            where TButton : IWebButton
    {
        string ControllerName { get; set; }

        string ActionName { get; set; }

        string ViewFolder { get; set; }

        string DefaultView { get; set; }
    }
}