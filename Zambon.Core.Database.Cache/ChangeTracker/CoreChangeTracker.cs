using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.Serialization.Formatters.Binary;
using Zambon.Core.Database.Cache.Helper;
using Zambon.Core.Database.Cache.Services;

namespace Zambon.Core.Database.Cache.ChangeTracker
{
    public class CoreChangeTracker
    {
        private readonly IDistributedCache cache;

        private readonly IInstanceKeyService instanceKeyService;

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


        private bool IsTracking(string modelType, int entityId, bool checkTemp = false)
        {
            return IsTracking(new StoreKey(modelType, entityId), checkTemp);
        }
        private bool IsTracking(StoreKey storeKey, bool checkTemp = false)
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
            return GetKey(storeKey.ModelType, storeKey.EntityId);
        }
        private string GetKey(string modelType, int entityId)
        {
            return $"{instanceKeyService.RetrieveKey().ToString()}_{modelType}_{entityId.ToString()}";
        }

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
            object entity = ctx.Find(modelType, storeKey.EntityId);
            if (storeKey.EntityId <= 0)
            {
                if (entity == null)
                {
                    entity = ctx.CreateProxy(modelType);
                    modelType.GetProperty("ID").SetValue(entity, storeKey.EntityId);
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


        public void Add<T>(EntityEntry<T> entry, bool saveToTemp = false) where T : class, ITrackableEntity
        {
            var objectKey = new StoreKey(entry.Metadata.ClrType.GetCorrectTypeName(), entry.Entity.ID);
            var key = GetKey(objectKey);

            var storedTempKeys = ReadTempStoredKeys().ToList();
            var storedKeys = ReadStoredKeys().ToList();

            var changedProperties = GetChanges(entry);
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
                        SetStoredKeys(storedTempKeys.ToArray());
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

        public void Remove(string modelType, int entityId, bool onlyFromTemp = false)
        {
            var objectKey = new StoreKey(modelType, entityId);
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
                SetStoredKeys((new StoreKey[0]));
            }
        }

        #endregion

        #region Save

        public void SaveObjects<T>(DbContext ctx, T entity) where T : class
        {
            EntityEntry entry;

            if (!(entity is ITrackableEntity) || ((entity is ITrackableEntity trackableEntity) && trackableEntity.ID <= 0))
            {
                entry = ctx.Add(entity);
                entry.State = EntityState.Added;
            }
            else
            {
                entry = ctx.Attach(entity);

                var modifiedCount = 0;
                var dbValues = entry.GetDatabaseValues();
                foreach (var property in dbValues.Properties)
                {
                    var dataTypes = property.PropertyInfo.GetCustomAttributes(typeof(DataTypeAttribute), true);
                    var isPassword = dataTypes.Count() > 0 && dataTypes.FirstOrDefault() is DataTypeAttribute dataType && dataType.DataType == DataType.Password;

                    if (property.Name != "Discriminator"
                        && ((isPassword && !string.IsNullOrWhiteSpace(entry.CurrentValues[property].ToString())) || !isPassword)
                        && ((entry.CurrentValues[property] == null && dbValues[property] != null) || (entry.CurrentValues[property] != null && !entry.CurrentValues[property].Equals(dbValues[property]))))
                    {
                        entry.Property(property.Name).IsModified = true;
                        modifiedCount++;
                    }
                }

                if (modifiedCount == 0)
                    entry.State = EntityState.Unchanged;

                foreach (var nav in entry.Navigations)
                    if (nav.CurrentValue is System.Collections.IEnumerable records)
                        foreach (var record in records)
                            if (record is ITrackableEntity childEntity && IsTracking(childEntity.GetType().GetCorrectTypeName(), childEntity.ID))
                                SaveObjects(ctx, childEntity);
            }
        }

        private static Dictionary<string, object> GetChanges<T>(EntityEntry<T> Entry) where T : class, ITrackableEntity
        {
            if (Entry.Entity.ID <= 0)
                return Entry.Properties.Where(x => x.Metadata.Name != "ID").ToDictionary(k => k.Metadata.Name, v => Entry.CurrentValues[v.Metadata.Name]);

            var dbData = Entry.GetDatabaseValues();
            return Entry.Properties.Where(

                x => x.Metadata.Name != "Discriminator" //TODO: Maybe check for ShadowProperties?
                    && ((Entry.CurrentValues[x.Metadata.Name] == null && dbData[x.Metadata.Name] != null)
                    || (Entry.CurrentValues[x.Metadata.Name] != null && !Entry.CurrentValues[x.Metadata.Name].Equals(dbData[x.Metadata.Name]))

                )).ToDictionary(k => k.Metadata.Name, v => Entry.CurrentValues[v.Metadata.Name]);
        }

        #endregion

    }
}