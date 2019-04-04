using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.ChangeTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Custom ChangeTracker used to track instances of all objects before actually saving in database.
    /// </summary>
    public class CoreChangeTracker
    {
        private readonly IDistributedCache cache;

        private readonly IInstanceKeyService instanceKeyService;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="_cache">Service for accessing the distributed cache.</param>
        /// <param name="_instanceKeyService">Service for accessing the instance key.</param>
        public CoreChangeTracker(IDistributedCache _cache, IInstanceKeyService _instanceKeyService)
        {
            cache = _cache;
            instanceKeyService = _instanceKeyService;
        }


        #region Read

        private StoreKey[] ReadTempStoredKeys()
        {
            return cache.Get("TEMP_" + instanceKeyService.RetrieveKey().ToString()).DeserializeStoreKeys();
        }

        private StoreKey[] ReadStoredKeys()
        {
            return cache.Get(instanceKeyService.RetrieveKey().ToString()).DeserializeStoreKeys();
        }


        /// <summary>
        /// Checks if the change tracker is already tracking the object instance.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="ctx">The database context instance.</param>
        /// <param name="entity">The entity instance.</param>
        /// <param name="checkTemp">By default will only check object with already applied values.</param>
        /// <returns>If the object is already being tracked returns true.</returns>
        public bool IsTracking<T>(DbContext ctx, T entity, bool checkTemp = false) where T : class, ITrackableEntity
        {
            return IsTracking(new StoreKey(ProxyUtil.GetUnproxiedType(entity).Name, ctx.GetKeyValues<T>(entity)));
        }

        /// <summary>
        /// Checks if the change tracker is already tracking the object instance.
        /// </summary>
        /// <param name="entry">The entity database entry.</param>
        /// <param name="checkTemp">By default will only check object with already applied values.</param>
        /// <returns>If the object is already being tracked returns true.</returns>
        public bool IsTracking(EntityEntry entry, bool checkTemp = false)
        {
            return IsTracking(new StoreKey(entry), checkTemp);
        }

        /// <summary>
        /// Checks if the change tracker is already tracking the object instance.
        /// </summary>
        /// <param name="storeKey">The database store key.</param>
        /// <param name="checkTemp">By default will only check object with already applied values.</param>
        /// <returns></returns>
        public bool IsTracking(StoreKey storeKey, bool checkTemp = false)
        {
            var exists = false;

            if (checkTemp)
                exists = ReadTempStoredKeys().Contains(storeKey);

            if (!exists)
                exists = ReadStoredKeys().Contains(storeKey);

            return exists;
        }


        private string GetKey(StoreKey storeKey)
        {
            return $"{instanceKeyService.RetrieveKey().ToString()}_{storeKey.ToString()}";
        }


        /// <summary>
        /// Read all objects in database and loads them into the EF DbContext.
        /// </summary>
        /// <param name="ctx"></param>
        public void LoadObjects(DbContext ctx)
        {
            var storedTempKeys = ReadTempStoredKeys();
            for (var t = 0; t < storedTempKeys.Length; t++)
            {
                var tempObject = cache.Get("TEMP_" + GetKey(storedTempKeys[t])).DeserializeStoredObject();
                LoadObject(ctx, storedTempKeys[t], tempObject);
            }

            var storedKeys = ReadStoredKeys();
            for (var k = 0; k < storedKeys.Length; k++)
            {
                if (!storedTempKeys.Contains(storedKeys[k]))
                {
                    var storedObject = cache.Get(GetKey(storedKeys[k])).DeserializeStoredObject();
                    LoadObject(ctx, storedKeys[k], storedObject);
                }
            }
        }

        private void LoadObject(DbContext ctx, StoreKey storeKey, Dictionary<string, object> storedObject)
        {
            var entityType = ctx.Model.GetEntityTypes().FirstOrDefault(x => x.ClrType.Name == storeKey.ModelType);
            var modelType = entityType.ClrType;

            EntityEntry entry;
            object entity = ctx.Find(modelType, storeKey.EntityKeys);
            if (entity == null)
            {
                if (entity == null)
                {
                    entity = ctx.CreateProxy(modelType);

                    var key = entityType.FindPrimaryKey();
                    for (var i = 0; i < key.Properties.Count; i++)
                        modelType.GetProperty(key.Properties[i].Name).SetValue(entity, storeKey.EntityKeys[i]);
                }

                entry = ctx.Add(entity);
                entry.State = EntityState.Added;
            }
            else
            {
                entry = ctx.Attach(entity);
                entry.State = EntityState.Modified;
            }

            entry.CurrentValues.SetValues(storedObject);
        }

        #endregion

        #region Write

        private void SetTempStoredKeys(StoreKey[] keys)
        {
            cache.Set("TEMP_" + instanceKeyService.RetrieveKey().ToString(), keys.SerializeStoreKeys());
        }

        private void SetStoredKeys(StoreKey[] keys)
        {
            cache.Set(instanceKeyService.RetrieveKey().ToString(), keys.SerializeStoreKeys());
        }


        /// <summary>
        /// Add an object into the ChangeTracker.
        /// </summary>
        /// <typeparam name="T">Must be a ITrackableEntity</typeparam>
        /// <param name="entry">The object instance.</param>
        /// <param name="saveToTemp">Should save in temporary edited objects or already applied changes.</param>
        public void Add<T>(EntityEntry<T> entry, bool saveToTemp = false) where T : class, ITrackableEntity
        {
            var objectKey = new StoreKey(entry);
            var key = GetKey(objectKey);

            var storedTempKeys = ReadTempStoredKeys().ToList();
            var storedKeys = ReadStoredKeys().ToList();

            var changedProperties = entry.GetChangedProperties();
            if ((changedProperties?.Count() ?? 0) > 0)
            {
                if (saveToTemp)
                {
                    if (!storedTempKeys.Contains(objectKey))
                    {
                        storedTempKeys.Add(objectKey);
                        SetTempStoredKeys(storedTempKeys.ToArray());
                    }
                    cache.Set("TEMP_" + key, changedProperties.SerializeStoredObject());
                }
                else
                {
                    if (storedTempKeys.Contains(objectKey))
                    {
                        storedTempKeys.Remove(objectKey);
                        SetTempStoredKeys(storedTempKeys.ToArray());
                        cache.Remove("TEMP_" + key);
                    }

                    if (!storedKeys.Contains(objectKey))
                    {
                        storedKeys.Add(objectKey);
                        SetStoredKeys(storedKeys.ToArray());
                    }
                    cache.Set(key, changedProperties.SerializeStoredObject());
                }
            }
            else
            {
                if (storedTempKeys.Contains(objectKey))
                    cache.Remove("TEMP_" + key);

                if (storedKeys.Contains(objectKey))
                    cache.Remove(key);
            }
        }

        /// <summary>
        /// Removes an object from ChangeTracker
        /// </summary>
        /// <param name="entry">The object instance.</param>
        /// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        public void Remove(EntityEntry entry, bool onlyFromTemp = false)
        {
            Remove(new StoreKey(entry), onlyFromTemp);
        }

        /// <summary>
        /// Removes an object from ChangeTracker
        /// </summary>
        /// <param name="objectKey">The object key.</param>
        /// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        public void Remove(StoreKey objectKey, bool onlyFromTemp = false)
        {
            //var objectKey = new StoreKey(entry);
            var key = GetKey(objectKey);

            var storedTempKeys = ReadTempStoredKeys().ToList();
            if (storedTempKeys.Contains(objectKey))
            {
                storedTempKeys.Remove(objectKey);
                SetTempStoredKeys(storedTempKeys.ToArray());
                cache.Remove("TEMP_" + key);
            }

            if (!onlyFromTemp)
            {
                var storedKeys = ReadStoredKeys().ToList();
                if (storedKeys.Contains(objectKey))
                {
                    storedKeys.Remove(objectKey);
                    SetStoredKeys(storedTempKeys.ToArray());
                    cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Clears the stored entities in distributed cache.
        /// </summary>
        /// <param name="clearStored">If should clear the stored entities with changes already applied.</param>
        /// <param name="clearTemp">If should clear the temp stored entities.</param>
        /// <param name="tempModelType">Filters the temp model to clean only this same type.</param>
        /// <param name="forceClear">Force to clean all entities in temp.</param>
        public void Clear(bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        {
            if (clearTemp)
            {
                var remainingKeys = new List<StoreKey>();

                var storedTempKeys = ReadTempStoredKeys();
                for (var t = 0; t < storedTempKeys.Length; t++)
                {
                    if (forceClear || storedTempKeys[t].ModelType == tempModelType)
                    {
                        var key = GetKey(storedTempKeys[t]);
                        cache.Remove("TEMP_" + key);
                    }
                    else
                        remainingKeys.Add(storedTempKeys[t]);
                }
                SetTempStoredKeys(remainingKeys.ToArray());
            }

            if (clearStored)
            {
                var storedKeys = ReadStoredKeys();
                for (var k = 0; k < storedKeys.Length; k++)
                {
                    var key = GetKey(storedKeys[k]);
                    cache.Remove(key);
                }
                SetStoredKeys(new StoreKey[0]);
            }
        }

        #endregion
    }
}