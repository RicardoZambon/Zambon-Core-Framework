using Zambon.Core.Module.Model.Abstractions;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebViewsParentBase
        <TDetailView, TListView, TLookupView,
        TSearchPropertiesParent, TSearchProperty,
        TButtonsParent, TButton,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate>
            : ViewsParentBase<TDetailView, TListView, TLookupView, TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TDetailView : WebDetailViewBase<TButtonsParent, TButton>
        where TListView : WebListViewBase<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TLookupView : WebLookupViewBase<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>

        where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
            where TSearchProperty : SearchPropertyBase
        where TButtonsParent : WebButtonsParentBase<TButton>
            where TButton : WebButtonBase
        where TColumnsParent : ColumnsParentBase<TColumn>
            where TColumn : ColumnBase
        where TGridTemplatesParent : WebGridTemplatesParentBase<TGridTemplate>
            where TGridTemplate : WebGridTemplateBase
    {
        #region Constructors

        public WebViewsParentBase() : base()
        {
        }

        #endregion
    }
}