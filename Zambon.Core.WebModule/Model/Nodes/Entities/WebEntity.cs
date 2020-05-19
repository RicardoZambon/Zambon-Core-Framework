using Microsoft.EntityFrameworkCore.Metadata;
using System.Xml.Serialization;
using Zambon.Core.Module.Model.Nodes.EntityTypes;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Nodes.Entities
{
    public class WebEntity<TProperty> : Entity<TProperty>, IWebEntity where TProperty : class, Module.Interfaces.Models.IProperty
    {
        #region XML Attributes

        [XmlAttribute]
        public string DefaultController { get; set; }

        #endregion

        #region Constructors

        public WebEntity() : base()
        {
        }

        public WebEntity(IEntityType dbEntity) : base(dbEntity)
        {
        }

        #endregion
    }
}