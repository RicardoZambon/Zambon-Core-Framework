using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views.SearchProperties
{
    public class SearchProperty : SerializeNodeBase, ISearchProperty
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string PropertyName { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        #endregion

        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}