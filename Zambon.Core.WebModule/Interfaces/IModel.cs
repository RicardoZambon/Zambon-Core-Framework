using System;

namespace Zambon.Core.Database.WebModule.Interfaces
{
    /// <summary>
    /// Interface used to identify model objects.
    /// </summary>
    /// <typeparam name="TEntity">Object database entity type.</typeparam>
    public interface IModel<TEntity> : IModel, ICustomValidatableObject where TEntity : class
    {
        /// <summary>
        /// Maps custom properties from database entity with the model.
        /// </summary>
        /// <param name="entity">The database object instance.</param>
        void MapFromEntity(TEntity entity);

        /// <summary>
        /// Maps custom properties from model to the database entity.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="entity">The database object instance.</param>
        void MapToEntity(IServiceProvider serviceProvider, TEntity entity);
    }

    /// <summary>
    /// Interface used to identify model objects.
    /// </summary>
    public interface IModel
    {
        void MapFromEntity(object entity);

        void MapToEntity(IServiceProvider serviceProvider, object entity);
    }
}