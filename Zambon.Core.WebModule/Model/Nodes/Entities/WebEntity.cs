using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Nodes.EntityTypes;
using Zambon.Core.WebModule.Interfaces.Models;
using Zambon.Core.WebModule.Model.Abstractions;

namespace Zambon.Core.WebModule.Model.Nodes.Entities
{
    public class WebEntity<TProperty> : Entity<TProperty>, IWebEntity
        where TProperty : class, IProperty
    {
        #region XML Attributes

        [XmlAttribute]
        public string DefaultController { get; set; }

        #endregion
    }
}