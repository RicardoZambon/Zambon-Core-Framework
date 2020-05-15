using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class PropertiesBase<TProperty> : BaseNode, IProperties<TProperty> where TProperty : IProperty
    {
        #region Constants

        private const string PROPERTIES = "Property";

        #endregion

        #region XML Attributes

        [XmlElement(PROPERTIES)]
        public ChildItemCollection<TProperty> PropertiesList { get; set; }

        #endregion

        #region Constructors

        public PropertiesBase()
        {
            PropertiesList = new ChildItemCollection<TProperty>(this);
        }

        #endregion
    }
}