using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class GridTemplatesParentBase<TGridTemplate> : SerializeNodeBase, IGridTemplatesParent<TGridTemplate>
        where TGridTemplate : GridTemplateBase
    {
        #region Constants

        private const string GRID_TEMPLATES_NODE = "GridTemplate";

        #endregion

        #region XML Elements

        [XmlElement(GRID_TEMPLATES_NODE)]
        public ChildItemCollection<TGridTemplate> GridTemplatesList { get; set; }

        #endregion

        #region Constructors

        public GridTemplatesParentBase()
        {
            GridTemplatesList = new ChildItemCollection<TGridTemplate>(this);
        }

        #endregion
    }
}