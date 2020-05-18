using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebButton : IButton
    {
        string CssClass { get; set; }

        string ControllerName { get; set; }

        string ActionName { get; set; }

        string ActionParameters { get; set; }

        string ActionMethod { get; set; }

        bool? UseFormPost { get; set; }
    }
}