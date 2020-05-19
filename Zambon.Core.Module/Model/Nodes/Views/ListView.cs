using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Nodes.Views.Buttons;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public class ListView<TSearchProperty, TButton, TColumn, TGridTemplate> : ViewResultSetBase<TSearchProperty, TColumn, TGridTemplate>, IListView<TSearchProperty, TButton, TColumn, TGridTemplate>
        where TSearchProperty : class, ISearchProperty
        where TButton : class, IButton
        where TColumn : class, IColumn
        where TGridTemplate : class, IGridTemplate
    {
        #region XML Arrays

        [XmlArray, XmlArrayItem(nameof(Button))]
        public ChildItemCollection<TButton> Buttons { get; set; }

        #endregion

        #region Constructors

        public ListView()
        {
            Buttons = new ChildItemCollection<TButton>(this);
        }

        #endregion
    }
}