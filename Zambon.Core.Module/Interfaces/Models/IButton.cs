using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IButton : IParent, IIcon, IIndex, ICondition
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string Target { get; set; }

        string ConfirmationMessage { get; set; }
    }
}