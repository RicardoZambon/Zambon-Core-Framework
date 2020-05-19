using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Interfaces.Models.Common;
using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.GridTemplates;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public class ViewResultSetBase<TSearchProperty, TColumn, TGridTemplate> : ViewBase, IViewResultSet<TSearchProperty, TColumn, TGridTemplate>
        where TSearchProperty : class, ISearchProperty
        where TColumn : class, IColumn
        where TGridTemplate : class, IGridTemplate
    {
        #region XML Attributes

        [XmlAttribute]
        public string Criteria { get; set; }

        [XmlAttribute]
        public string CriteriaArguments { get; set; }

        [XmlAttribute]
        public string Sort { get; set; }

        #endregion

        #region XML Arrays

        [XmlArray, XmlArrayItem(nameof(SearchProperty))]
        public ChildItemCollection<TSearchProperty> SearchProperties { get; set; }

        [XmlArray, XmlArrayItem(nameof(Column))]
        public ChildItemCollection<TColumn> Columns { get; set; }

        [XmlArray, XmlArrayItem(nameof(GridTemplate))]
        public ChildItemCollection<TGridTemplate> GridTemplates { get; set; }

        #endregion

        #region Constructors

        public ViewResultSetBase()
        {
            SearchProperties = new ChildItemCollection<TSearchProperty>(this);
            Columns = new ChildItemCollection<TColumn>(this);
            GridTemplates = new ChildItemCollection<TGridTemplate>(this);
        }

        #endregion
    }
}