using System.Xml.Serialization;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public class WebListViewBase
        <TSearchPropertiesParent, TSearchProperty,
        TButtonsParent, TButton,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate>
            : ListViewBase<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>,
                IWebListView<TSearchPropertiesParent, TSearchProperty, TButtonsParent, TButton, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
            where TSearchProperty : SearchPropertyBase
        where TButtonsParent : ButtonsParentBase<TButton>
            where TButton : WebButtonBase
        where TColumnsParent : ColumnsParentBase<TColumn>
            where TColumn : ColumnBase
        where TGridTemplatesParent : GridTemplatesParentBase<TGridTemplate>
            where TGridTemplate : WebGridTemplateBase
    {
        #region XML Attributes

        [XmlAttribute]
        public string ControllerName { get; set; }

        [XmlAttribute]
        public string ActionName { get; set; }

        #endregion
    }
}