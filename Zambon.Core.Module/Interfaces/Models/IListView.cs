using Zambon.Core.Module.Interfaces.Models.Common;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IListView<
        TSearchPropertiesParent, TSearchProperty,
        TButtonsParent, TButton,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate> : IView, IViewResultSet<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>, IViewButtons<TButtonsParent, TButton>
        where TSearchPropertiesParent : ISearchPropertiesParent<TSearchProperty>
            where TSearchProperty : ISearchProperty
        where TButtonsParent : IButtonsParent<TButton>
            where TButton : IButton
        where TColumnsParent : IColumnsParent<TColumn>
            where TColumn : IColumn
        where TGridTemplatesParent : IGridTemplatesParent<TGridTemplate>
            where TGridTemplate : IGridTemplate
    {
    }
}