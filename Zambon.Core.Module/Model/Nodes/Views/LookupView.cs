using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public class LookupView<TSearchProperty, TColumn, TGridTemplate> : ViewResultSetBase<TSearchProperty, TColumn, TGridTemplate>, ILookupView<TSearchProperty, TColumn, TGridTemplate>
        where TSearchProperty : class, ISearchProperty
        where TColumn : class, IColumn
        where TGridTemplate : class, IGridTemplate
    {
    }
}