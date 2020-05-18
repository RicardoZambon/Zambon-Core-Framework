using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IListView : IView
    {
    }

    public interface IListView<TSearchProperty, TButton, TColumn, TGridTemplate> : IListView, IViewResultSet<TSearchProperty, TColumn, TGridTemplate>, IViewButtons<TButton>
        where TSearchProperty : ISearchProperty
        where TButton : IButton
        where TColumn : IColumn
        where TGridTemplate : IGridTemplate
    {
    }
}