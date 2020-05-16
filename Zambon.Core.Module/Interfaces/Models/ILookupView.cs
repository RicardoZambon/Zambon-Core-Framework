using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface ILookupView<
        TSearchPropertiesParent, TSearchProperty,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate> : IView, IViewResultSet<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TSearchPropertiesParent : ISearchPropertiesParent<TSearchProperty>
            where TSearchProperty : ISearchProperty
        where TColumnsParent : IColumnsParent<TColumn>
            where TColumn : IColumn
        where TGridTemplatesParent : IGridTemplatesParent<TGridTemplate>
            where TGridTemplate : IGridTemplate
    {
    }
}