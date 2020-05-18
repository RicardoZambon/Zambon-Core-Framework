using Zambon.Core.Module.Model.Abstractions;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebLookupViewBase
        <TSearchPropertiesParent, TSearchProperty,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate>
            : LookupViewBase<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
            where TSearchProperty : SearchPropertyBase
        where TColumnsParent : ColumnsParentBase<TColumn>
            where TColumn : ColumnBase
        where TGridTemplatesParent : WebGridTemplatesParentBase<TGridTemplate>
            where TGridTemplate : WebGridTemplateBase
    {
    }
}