using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class StaticTextBase : SerializeNodeBase, IStaticText
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        #endregion
    }
}