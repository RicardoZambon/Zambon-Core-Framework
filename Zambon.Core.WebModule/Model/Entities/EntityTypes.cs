using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.WebModule.Model.Entities
{
    public class EntityTypes : Module.Model.Entities.EntityTypes
    {
        #region XML Attributes

        private ChildItemCollection<Entity> entitiesList;
        /// <summary>
        /// List of all entities available in XML model.
        /// </summary>
        [XmlElement(nameof(Entity)), XmlOverride]
        public new ChildItemCollection<Entity> EntitiesList
        {
            get => base.EntitiesList;
            set => base.EntitiesList = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EntityTypes()
        {
            EntitiesList = new ChildItemCollection<Entity>(this);
        }

        #endregion

    }
}