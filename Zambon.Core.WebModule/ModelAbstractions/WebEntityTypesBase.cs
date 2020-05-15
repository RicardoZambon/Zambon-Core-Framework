using Zambon.Core.Module.ModelAbstractions;

namespace Zambon.Core.WebModule.ModelAbstractions
{
    public abstract class WebEntityTypesBase<TEntity> : EntityTypesBase<TEntity> where TEntity : WebEntityBase
    {
    }
}