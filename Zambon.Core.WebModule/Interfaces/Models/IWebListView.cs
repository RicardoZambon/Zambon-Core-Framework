using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.WebModule.Interfaces.Models
{
    public interface IWebListView<TSearchProperty, TButton, TColumn, TGridTemplate> : IListView<TSearchProperty, TButton, TColumn, TGridTemplate>
        where TSearchProperty : ISearchProperty
        where TButton : IWebButton
        where TColumn : IColumn
        where TGridTemplate : IWebGridTemplate
    {
        string ControllerName { get; set; }

        string ActionName { get; set; }
    }
}