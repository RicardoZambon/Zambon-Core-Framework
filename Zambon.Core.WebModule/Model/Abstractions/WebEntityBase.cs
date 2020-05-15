using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.ModelAbstractions
{
    public abstract class WebEntityBase<TProperties, TProperty> : EntityBase<TProperties, TProperty>, IWebEntity
        where TProperties : IProperties<TProperty>
            where TProperty : IProperty
    {
        #region XML Attributes

        [XmlAttribute]
        public string DefaultController { get; set; }

        #endregion
    }
}