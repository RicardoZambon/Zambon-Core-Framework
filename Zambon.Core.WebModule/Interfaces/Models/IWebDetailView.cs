using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebDetailView<TButton> : IDetailView<TButton> where TButton : IWebButton<TButton>
    {
        string ControllerName { get; set; }

        string ActionName { get; set; }

        string ViewFolder { get; set; }

        string DefaultView { get; set; }
    }
}