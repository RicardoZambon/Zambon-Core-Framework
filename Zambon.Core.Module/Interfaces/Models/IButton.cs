
using Zambon.Core.Module.Interfaces.Models.Common;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IButton<TSubButton> : IParent, IIcon, IIndex, ICondition
        where TSubButton : IButton<TSubButton>
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string Target { get; set; }

        string ConfirmationMessage { get; set; }

        ChildItemCollection<TSubButton> SubButtons { get; set; }
    }
}