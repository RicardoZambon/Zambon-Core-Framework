using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.StaticTexts
{
    public class StaticText : SerializeNodeBase, IStaticText
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string Key { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}