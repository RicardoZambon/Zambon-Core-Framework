using Zambon.Core.Module.Interfaces.Models;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class LookupViewBase
        <TSearchPropertiesParent, TSearchProperty,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate>
            : ViewResultSetBase<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>,
                ILookupView<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
            where TSearchProperty : SearchPropertyBase
        where TColumnsParent : ColumnsParentBase<TColumn>
            where TColumn : ColumnBase
        where TGridTemplatesParent : GridTemplatesParentBase<TGridTemplate>
            where TGridTemplate : GridTemplateBase
    {
    }
}