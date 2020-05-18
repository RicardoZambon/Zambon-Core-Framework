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
        where TDetailView : DetailViewBase<TButtonsParent, TButton>
        where TListView : ListViewBase<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TLookupView : LookupViewBase<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>

        where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
            where TSearchProperty : SearchPropertyBase
        where TButtonsParent : ButtonsParentBase<TButton>
            where TButton : ButtonBase
        where TColumnsParent : ColumnsParentBase<TColumn>
            where TColumn : ColumnBase
        where TGridTemplatesParent : GridTemplatesParentBase<TGridTemplate>
            where TGridTemplate : GridTemplateBase
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