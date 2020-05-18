using Zambon.Core.Module.Model.Abstractions;

namespace Zambon.Core.WebModule.Model.Abstractions
{
    public abstract class WebEntityTypesParentBase<TEntity, TPropertiesParent, TProperty> : EntityTypesParentBase<TEntity, TPropertiesParent, TProperty>
        where TEntity : WebEntityBase<TPropertiesParent, TProperty>
            where TPropertiesParent : PropertiesParentBase<TProperty>
                where TProperty : PropertyBase
    {
        #region Constructors

        public WebEntityTypesParentBase() : base()
        {
        }

        #endregion
    }
}