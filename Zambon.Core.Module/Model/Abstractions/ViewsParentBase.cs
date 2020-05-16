using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ViewsParentBase
        <TDetailView, TListView, TLookupView,
            TSearchPropertiesParent, TSearchProperty,
            TButtonsParent, TButton,
            TColumnsParent, TColumn,
            TGridTemplatesParent, TGridTemplate>
            : SerializeNodeBase,
                IViewsParent<TDetailView, TListView, TLookupView, TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
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
        #region XML Elements

        public ChildItemCollection<TDetailView> DetailViews { get; set; }

        public ChildItemCollection<TListView> ListViews { get; set; }

        public ChildItemCollection<TLookupView> LookupViews { get; set; }

        #endregion

        #region Constructors

        public ViewsParentBase()
        {
            DetailViews = new ChildItemCollection<TDetailView>(this);
            ListViews = new ChildItemCollection<TListView>(this);
            LookupViews = new ChildItemCollection<TLookupView>(this);
        }

        #endregion
    }
}