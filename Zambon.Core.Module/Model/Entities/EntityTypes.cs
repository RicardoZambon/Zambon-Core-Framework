using System.Xml.Serialization;
using Zambon.Core.Module.ModelAbstractions;

namespace Zambon.Core.Module.Model.Entities
{
    /// <summary>
    /// Represents a list of entity types in XML model file.
    /// </summary>
    [XmlType(AnonymousType = true)]
    public class EntityTypes : EntityTypesBase<Entity>
    {
        //#region XML Attributes

        ///// <summary>
        ///// List of all entities available in XML model.
        ///// </summary>
        //[XmlElement(nameof(Entity))]
        //public ChildItemCollection<Entity> EntitiesList { get; set; }

        //#endregion

        //#region Constructors

        ///// <summary>
        ///// Default constructor.
        ///// </summary>
        //public EntityTypes()
        //{
        //    EntitiesList = new ChildItemCollection<Entity>(this);
        //}

        //#endregion
    }
}