using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebEntityBase<TPropertiesParent, TProperty> : EntityBase<TPropertiesParent, TProperty>, IWebEntity
        where TPropertiesParent : IPropertiesParent<TProperty>
            where TProperty : IProperty
    {
        #region XML Attributes

        [XmlAttribute]
        public string DefaultController { get; set; }

        #endregion
    }
}