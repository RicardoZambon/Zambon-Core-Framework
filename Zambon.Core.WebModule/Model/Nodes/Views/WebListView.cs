using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Nodes.Views;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Nodes.Views
{
    public sealed class WebListView<TSearchProperty, TButton, TColumn, TGridTemplate> : ListView<TSearchProperty, TButton, TColumn, TGridTemplate>, IWebListView<TSearchProperty, TButton, TColumn, TGridTemplate>
        where TSearchProperty : class, ISearchProperty
        where TButton : class, IWebButton<TButton>
        where TColumn : class, IColumn
        where TGridTemplate : class, IWebGridTemplate
    {
        #region XML Attributes

        [XmlAttribute]
        public string ControllerName { get; set; }

        [XmlAttribute]
        public string ActionName { get; set; }

        #endregion
    }
}