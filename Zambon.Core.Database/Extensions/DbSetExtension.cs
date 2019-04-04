using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.Domain.Entities;
using Zambon.Core.Database.Domain.Interfaces;

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
        /// <param name="ctx">Database context instance.</param>
        /// <param name="id">ID of the object to be deleted.</param>
        /// <param name="_commitChanges">If should or not use transaction when deleting.</param>
        /// <returns>Returns an instance of the deleted object.</returns>
        public static T Delete<T>(this CoreDbContext ctx, int id, bool _commitChanges = true) where T : class, IEntity, IKeyed
        {
            var entity = ctx.Find<T>(id);
            if (entity != null)
            {
                if (entity is ITrackableEntity entityTrackable)
                {
                    if (entity.ID > 0)
                    {
                        if (entity is DBObject entityDBObject)
                        {
                            entityDBObject.IsDeleted = true;

                            if (_commitChanges)
                                ctx.SaveChanges(entityDBObject);
                            else
                                ctx.ApplyChanges(entityTrackable);
                        }
                        else
                        {
                            ctx.Remove(entityTrackable);
                            ctx.SaveChanges();
                        }
                    }
                    else
                        ctx.RemoveTrackedEntity(entity);
                }
                else
                {
                    ctx.Remove(entity);
                    ctx.SaveChanges();
                }

                return entity;
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Validate the an entity.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">Instance of the object.</param>
        /// <param name="ctx">Database context instance.</param>
        /// <param name="errors">Exposes the errors list if having any.</param>
        /// <returns>Returns true if no errors were found.</returns>
        public static bool IsValid<T>(this T entity, CoreDbContext ctx, out List<KeyValuePair<string, string>> errors) where T : ICustomValidated
        {
            errors = entity.ValidateData(new[] { ctx });
            return errors.Count == 0;
        }

        /// <summary>
        /// Merge the actual database values with the object from Core Change Tracker.
        /// </summary>
        /// <param name="ctx">Database context instance.</param>
        /// <param name="modalEntity">The instance returned from the action.</param>
        /// <param name="formKeys">List of user informed fields.</param>
        /// <returns>Returns the merged entity.</returns>
        public static object Merge(this CoreDbContext ctx, IKeyed modalEntity, IEnumerable<string> formKeys)
        {
            var type = modalEntity.GetType();
            var dbEntity = ctx.Entry(ctx.Find(type, modalEntity.ID));

            var changedProperties = formKeys.Where(x => type.GetProperty(x) != null).ToDictionary(k => k, v => type.GetProperty(v).GetValue(modalEntity));
            if (changedProperties != null && changedProperties.Count() > 0)
                dbEntity.CurrentValues.SetValues(changedProperties.ToDictionary(k => k.Key, v => v.Value));

            if (changedProperties.Count(x => dbEntity.Properties.Count(y => y.Metadata.Name == x.Key) == 0) > 0)
            {   //Set value for unmapped properties in DB
                var unmappedProperties = changedProperties.Where(x => dbEntity.Properties.Count(y => y.Metadata.Name == x.Key) == 0);
                foreach (var property in unmappedProperties)
                {
                    var prop = dbEntity.Entity.GetType().GetProperty(property.Key);
                    if (prop.SetMethod != null) //Checks if it has set method
                        prop.SetValue(dbEntity.Entity, property.Value);
                }
            }

            return dbEntity.Entity;
        }
    }
}