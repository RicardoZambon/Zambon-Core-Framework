using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class PropertiesParentBase<TProperty> : SerializeNodeBase, IPropertiesParent<TProperty> where TProperty : IProperty
    {
        #region Constants

        private const string PROPERTIES_NODE = "Property";

        #endregion

        #region XML Attributes

        [XmlElement(PROPERTIES_NODE)]
        public ChildItemCollection<TProperty> PropertiesList { get; set; }

        #endregion

        #region Constructors

        public PropertiesParentBase()
        {
            PropertiesList = new ChildItemCollection<TProperty>(this);
        }

        #endregion
    }
}