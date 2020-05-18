using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ILookupView : IView
    {
    }

    public interface ILookupView<TSearchProperty, TColumn, TGridTemplate> : ILookupView, IViewResultSet<TSearchProperty, TColumn, TGridTemplate>
        where TSearchProperty : ISearchProperty
        where TColumn : IColumn
        where TGridTemplate : IGridTemplate
    {
    }
}