using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IViewsParent<TDetailView, TListView, TLookupView,
        TSearchPropertiesParent, TSearchProperty,
        TButtonsParent, TButton,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate> : IParent
        where TDetailView : IDetailView<TButtonsParent, TButton>
        where TListView : IListView<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TLookupView : ILookupView<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>

        where TSearchPropertiesParent : ISearchPropertiesParent<TSearchProperty>
            where TSearchProperty : ISearchProperty
        where TButtonsParent : IButtonsParent<TButton>
            where TButton : IButton
        where TColumnsParent : IColumnsParent<TColumn>
            where TColumn : IColumn
        where TGridTemplatesParent : IGridTemplatesParent<TGridTemplate>
            where TGridTemplate : IGridTemplate
    {
        ChildItemCollection<TDetailView> DetailViews { get; set; }

        ChildItemCollection<TListView> ListViews { get; set; }

        ChildItemCollection<TLookupView> LookupViews { get; set; }
    }
}