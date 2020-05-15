using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public class SearchPropertiesParentBase<TSearchProperty> : SerializeNodeBase, ISearchPropertiesParent<TSearchProperty>
        where TSearchProperty : SearchPropertyBase
    {
        #region Constants

        private const string SEARCH_PROPERTY_NODE = "SearchProperty";

        #endregion

        #region XML Elements

        [XmlElement(SEARCH_PROPERTY_NODE)]
        public ChildItemCollection<TSearchProperty> SearchPropertiesList { get; set; }

        #endregion

        #region Constructors

        public SearchPropertiesParentBase()
        {
            SearchPropertiesList = new ChildItemCollection<TSearchProperty>(this);
        }

        #endregion
    }
}