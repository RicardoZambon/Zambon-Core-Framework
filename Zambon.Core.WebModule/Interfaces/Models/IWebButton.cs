using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebButton<TSubButton> : IButton<TSubButton>
        where TSubButton : IButton<TSubButton>
    {
        string CssClass { get; set; }

        string ControllerName { get; set; }

        string ActionName { get; set; }

        string ActionParameters { get; set; }

        string ActionMethod { get; set; }

        bool? UseFormPost { get; set; }
    }
}