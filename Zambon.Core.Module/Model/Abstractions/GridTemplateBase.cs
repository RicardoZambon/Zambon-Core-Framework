using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class GridTemplateBase : SerializeNodeBase, IGridTemplate
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute, MergeKey]
        public string ColumnIds { get; set; }

        [XmlAttribute, MergeKey]
        public string Condition { get; set; }

        [XmlAttribute, MergeKey]
        public string ConditionArguments { get; set; }

        #endregion
    }
}