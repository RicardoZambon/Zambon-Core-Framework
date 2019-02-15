using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Database.Operations
{
    public static class DbSetExtension
    {

        #region Create / Update

        public static bool IsValid<T>(this T entity, CoreContext ctx, out List<KeyValuePair<string, string>> errors) where T : IDBObject
        {
            errors = entity.ValidateData(ctx);
            return errors.Count == 0;
        }

        #endregion


        #region Delete

        public static T Delete<T>(this CoreContext ctx, int id, bool _commitChanges = true) where T : class, ITrackableEntity
        {
            var entity = ctx.Find<T>(id);
            if (entity != null)
            {
                if (entity.ID > 0)
                {
                    if (entity is DBObject entityDBObject)
                    {
                        entityDBObject.IsDeleted = true;

                        if (_commitChanges)
                            ctx.SaveChanges(entity);
                        else
                            ctx.ApplyChanges(entity);
                    }
                    else
                    {
                        ctx.Remove(entity);
                        ctx.SaveChanges();
                    }
                }
                else
                    ctx.RemoveTrackedEntity(entity);

                return entity;
            }
            throw new KeyNotFoundException();
        }

        #endregion


        #region Read

        public static object Merge(this CoreContext ctx, object modalEntity, IEnumerable<string> formKeys)
        {
            var type = modalEntity.GetType();
            var id = ((BaseDBObject)modalEntity).ID;

            var dbEntity = ctx.Entry(ctx.Find(type, id));

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

        #endregion

    }
}