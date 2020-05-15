using System;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IMenu<TSubMenu> : IParent, IComparable
        where TSubMenu : IMenu<TSubMenu>
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string Icon { get; set; }

        int? Index { get; set; }

        string Type { get; set; }

        string ViewID { get; set; }

        bool? ShowBadge { get; set; }

        string BadgeQuery { get; set; }

        string BadgeQueryArguments { get; set; }


        ChildItemCollection<TSubMenu> SubMenus { get; set; }
    }
}