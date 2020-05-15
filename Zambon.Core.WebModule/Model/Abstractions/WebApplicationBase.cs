using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.WebModule.Interfaces.Models;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public class WebApplicationBase<TEntityTypes, TEntity, TProperties, TProperty> : ApplicationBase<TEntityTypes, TEntity, TProperties, TProperty>
        where TEntityTypes : IEntityTypes<TEntity, TProperties, TProperty>
            where TEntity : IEntity<TProperties, TProperty>, IWebEntity
                where TProperties : IProperties<TProperty>
                    where TProperty : IProperty
    {
    }
}
