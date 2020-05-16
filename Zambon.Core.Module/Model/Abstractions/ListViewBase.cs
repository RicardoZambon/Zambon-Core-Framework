using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ListViewBase
        <TSearchPropertiesParent, TSearchProperty,
        TButtonsParent, TButton,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate>
            : ViewResultSetBase<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>,
                IListView<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
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

        private TButtonsParent _buttons;
        [XmlElement(nameof(Buttons)), Browsable(false)]
        public TButtonsParent _Buttons
        {
            get => _buttons;
            set => SetParent(value, ref _buttons);
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public ChildItemCollection<TButton> Buttons => _Buttons?.ButtonsList ?? new ChildItemCollection<TButton>(null);

        #endregion
    }
}
