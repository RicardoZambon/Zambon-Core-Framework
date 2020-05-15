using Zambon.Core.Module.ModelAbstractions;
using Zambon.Core.WebModule.Model.Entities;

namespace Zambon.Core.WebModule.Model
{
    public class WebApplication : ApplicationBase<WebEntityTypes, WebEntity>
    {
        //#region XML Elements

        //private Entities.EntityTypes _entityTypes;
        ///// <summary>
        ///// Represents a list of entity types in XML model file.
        ///// </summary>
        //[XmlElement(nameof(EntityTypes)), Browsable(false), XmlOverride]
        //public new Entities.EntityTypes _EntityTypes
        //{
        //    get => _entityTypes;
        //    set { SetParent(value, ref _entityTypes); base._EntityTypes = _entityTypes; }
        //}

        //#endregion

        //#region Properties

        ///// <summary>
        ///// Represents a list of entity types in XML model file.
        ///// </summary>
        //[XmlIgnore]
        //public new ChildItemCollection<Entity> Entities => _EntityTypes?.EntitiesList ?? new ChildItemCollection<Entity>(null);

        //#endregion
    }
}