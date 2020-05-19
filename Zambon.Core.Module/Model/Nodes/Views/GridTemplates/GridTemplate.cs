using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views.GridTemplates
{
    public class GridTemplate : SerializeNodeBase, IGridTemplate
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute]
        public string ColumnIds { get; set; }

        [XmlAttribute]
        public string Condition { get; set; }

        [XmlAttribute]
        public string ConditionArguments { get; set; }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}