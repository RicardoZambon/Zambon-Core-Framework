using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Database.ExtensionMethods
{
    /// <summary>
    /// Extension methods to manipulate database objects.
    /// </summary>
    public static class DbSetExtension
    {
        /// <summary>
        /// Deletes an object from database. If the object is of type DBObject, then will only set the IsDeleted property to true, otherwise will delete.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="dbContext">Database context instance.</param>
        /// <param name="id">ID of the object to be deleted.</param>
        /// <param name="_commitChanges">If should or not use transaction when deleting.</param>
        /// <returns>Returns an instance of the deleted object.</returns>
        public static T Delete<T>(this CoreDbContext dbContext, int id, bool _commitChanges = true) where T : class
        {
            var entity = dbContext.Find<T>(id);
            if (entity != null)
            {
                if (entity is ITrackableEntity entityTrackable)
                {
                    if (!dbContext.IsNewEntry(entity))
                    {
                        if (entity is ISoftDelete entityDBObject)
                        {
                            entityDBObject.IsDeleted = true;
                            if (_commitChanges)
                            {
                                dbContext.CommitChanges(entity);
                            }
                            else
                            {
                                dbContext.ApplyChanges(entityTrackable);
                            }
                        }
                        else
                        {
                            if (entity is IDbObject dbObject)
                            {
                                dbObject.OnDeleting(dbContext);
                            }

                            dbContext.Remove(entityTrackable);
                            dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        if (entity is IDbObject dbObject)
                        {
                            dbObject.OnDeleting(dbContext);
                        }

                        dbContext.RemoveTrackedEntity(entity);
                    }
                }
                else
                {
                    if (entity is IDbObject dbObject)
                    {
                        dbObject.OnDeleting(dbContext);
                    }

                    dbContext.Remove(entity);
                    dbContext.SaveChanges();
                }
                return entity;
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Validate the an entity.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="serviceProvider">Service provider instance.</param>
        /// <param name="items">The error list.</param>
        /// <returns>Returns true if no errors were found.</returns>
        public static bool IsValid<T>(this T entity, IServiceProvider serviceProvider, out IDictionary<object, object> items) where T : ICustomValidatableObject
        {
            items = entity.ValidateData(new ValidationContext(entity, serviceProvider, new Dictionary<object, object>()));
            return items.Count == 0;
        }
    }
}