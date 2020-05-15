using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface INavigationParent<TMenu> : IParent
        where TMenu : IMenu<TMenu>
    {
        ChildItemCollection<TMenu> MenusList { get; set; }
    }
}