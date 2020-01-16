using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zambon.Core.Database.ChangeTracker.Services
{
    public class CoreChangeTrackerManager
    {
        private const string TempTrackPrefix = "TEMP_";

        #region Properties

        private readonly IDistributedCache cache;

        #endregion

        #region Constructors

        public CoreChangeTrackerManager(IDistributedCache _cache)
        {
            cache = _cache;
        }

        #endregion

        #region Methods - Read

        private StoredInstanceKey[] ReadTempStoredKeys(CacheKey cacheKey)
            => cache.Get(TempTrackPrefix + cacheKey.ToString()).DeserializeStoreKeys();

        private StoredInstanceKey[] ReadStoredKeys(CacheKey cacheKey)
            => cache.Get(cacheKey.ToString()).DeserializeStoreKeys();


        public void LoadAllTrackedInstances(DbContext dbContext, CacheKey cacheKey)
        {
            var storedTempKeys = ReadTempStoredKeys(cacheKey);
            for (var t = 0; t < storedTempKeys.Length; t++)
            {
                var tempObject = cache.Get(TempTrackPrefix + storedTempKeys[t].GetFullKey(cacheKey)).DeserializeStoredObject();
                dbContext.LoadProperties(storedTempKeys[t].EntityType, storedTempKeys[t].EntityKeys, tempObject);
            }

            var storedKeys = ReadStoredKeys(cacheKey);
            for (var k = 0; k < storedKeys.Length; k++)
            {
                if (!storedTempKeys.Contains(storedKeys[k]))
                {
                    var storedObject = cache.Get(storedKeys[k].ToString()).DeserializeStoredObject();
                    dbContext.LoadProperties(storedKeys[k].EntityType, storedKeys[k].EntityKeys, storedObject);
                }
            }
        }

        public IEnumerable<T> GetTrackedInstances<T>(DbContext dbContext, CacheKey cacheKey, bool readTemp) where T : class, ITrackableEntity
        {
            var loadedObjects = new List<T>();

            var storedTempKeys = new StoredInstanceKey[0];
            if (readTemp)
            {
                storedTempKeys = ReadTempStoredKeys(cacheKey);
                for (var t = 0; t < storedTempKeys.Length; t++)
                {
                    if (storedTempKeys[t].ModelType == dbContext.Model.FindEntityType(typeof(T)).RootType().Name)
                    {
                        var tempObject = cache.Get(TempTrackPrefix + storedTempKeys[t].GetFullKey(cacheKey)).DeserializeStoredObject();
                        loadedObjects.Add(dbContext.LoadProperties<T>(storedTempKeys[t].EntityKeys, tempObject));
                    }
                }
            }

            var storedKeys = ReadStoredKeys(cacheKey);
            for (var k = 0; k < storedKeys.Length; k++)
            {
                if (!storedTempKeys.Contains(storedKeys[k]) && storedKeys[k].ModelType == dbContext.Model.FindEntityType(typeof(T)).RootType().Name)
                {
                    var storedObject = cache.Get(storedKeys[k].GetFullKey(cacheKey)).DeserializeStoredObject();
                    loadedObjects.Add(dbContext.LoadProperties<T>(storedKeys[k].EntityKeys, storedObject));
                }
            }

            return loadedObjects;
        }


        public bool IsTracking(CacheKey cacheKey, StoredInstanceKey storedInstanceKey, bool checkTemp = false)
        {
            var exists = false;
            if (checkTemp)
            {
                exists = ReadTempStoredKeys(cacheKey).Contains(storedInstanceKey);
            }
            if (!exists)
            {
                exists = ReadStoredKeys(cacheKey).Contains(storedInstanceKey);
            }
            return exists;
        }

        #endregion

        #region Methods - Write

        private void AddTempStoredKey(CacheKey cacheKey, StoredInstanceKey storedInstanceKey, Dictionary<string, object> changedProperties)
        {
            if ((changedProperties?.Count() ?? 0) > 0)
            {
                var keys = ReadTempStoredKeys(cacheKey).ToList();
                if (keys.Contains(storedInstanceKey))
                {
                    keys.Remove(storedInstanceKey);
                }
                keys.Add(storedInstanceKey);
                cache.Set(TempTrackPrefix + cacheKey.ToString(), keys.ToArray().SerializeStoreKeys());
                cache.Set(TempTrackPrefix + storedInstanceKey.GetFullKey(cacheKey), changedProperties.SerializeStoredObject());
            }
            else
            {
                RemoveTempStoredKey(cacheKey, storedInstanceKey);
                RemoveStoredKey(cacheKey, storedInstanceKey);
            }
        }
        private void AddStoredKey(CacheKey cacheKey, StoredInstanceKey storedInstanceKey, Dictionary<string, object> changedProperties)
        {
            if ((changedProperties?.Count() ?? 0) > 0)
            {
                RemoveTempStoredKey(cacheKey, storedInstanceKey);

                var keys = ReadTempStoredKeys(cacheKey).ToList();
                if (keys.Contains(storedInstanceKey))
                {
                    keys.Remove(storedInstanceKey);
                }
                keys.Add(storedInstanceKey);
                cache.Set(storedInstanceKey.GetFullKey(cacheKey), changedProperties.SerializeStoredObject());
            }
            else
            {
                RemoveTempStoredKey(cacheKey, storedInstanceKey);
                RemoveStoredKey(cacheKey, storedInstanceKey);
            }
        }

        private void RemoveTempStoredKey(CacheKey cacheKey, StoredInstanceKey storedInstanceKey)
        {
            var keysArray = ReadTempStoredKeys(cacheKey);
            if (keysArray.Contains(storedInstanceKey))
            {
                var keys = keysArray.ToList();
                keys.Remove(storedInstanceKey);
                cache.Set(TempTrackPrefix + cacheKey.ToString(), keys.ToArray().SerializeStoreKeys());
                cache.Remove(TempTrackPrefix + storedInstanceKey.GetFullKey(cacheKey));
            }
        }
        private void RemoveStoredKey(CacheKey cacheKey, StoredInstanceKey storedInstanceKey)
        {
            var keysArray = ReadStoredKeys(cacheKey);
            if (keysArray.Contains(storedInstanceKey))
            {
                var keys = keysArray.ToList();
                keys.Remove(storedInstanceKey);
                cache.Set(cacheKey.ToString(), keys.ToArray().SerializeStoreKeys());
                cache.Remove(storedInstanceKey.GetFullKey(cacheKey));
            }
        }

        private void ClearTempStoredKeys(CacheKey cacheKey, string tempModelType, bool forceClear)
        {
            var remainingKeys = new List<StoredInstanceKey>();
            var tempKeys = ReadTempStoredKeys(cacheKey);
            for (var t = 0; t < tempKeys.Length; t++)
            {
                if (forceClear || tempKeys[t].ModelType == tempModelType)
                {
                    cache.Remove(TempTrackPrefix + tempKeys[t].GetFullKey(cacheKey));
                    break;
                }
                remainingKeys.Add(tempKeys[t]);
            }
            cache.Set(TempTrackPrefix + cacheKey.ToString(), remainingKeys.ToArray().SerializeStoreKeys());
        }
        private void ClearStoredKeys(CacheKey cacheKey)
        {
            var storedKeys = ReadStoredKeys(cacheKey);
            for (var k = 0; k < storedKeys.Length; k++)
            {
                cache.Remove(storedKeys[k].GetFullKey(cacheKey));
            }
            cache.Set(cacheKey.ToString(), new StoredInstanceKey[0].SerializeStoreKeys());
        }


        public void Add<T>(CacheKey cacheKey, EntityEntry<T> entry, bool saveToTemp = false) where T : class, ITrackableEntity
        {
            if (saveToTemp)
            {
                AddTempStoredKey(cacheKey, new StoredInstanceKey(entry, ChangeTrackerActions.InsertOrUpdate), entry.GetChangedProperties());
            }
            else
            {
                AddStoredKey(cacheKey, new StoredInstanceKey(entry, ChangeTrackerActions.InsertOrUpdate), entry.GetChangedProperties());
            }
        }

        public void Delete<T>(CacheKey cacheKey, EntityEntry<T> entry) where T : class, ITrackableEntity
        {
            AddStoredKey(cacheKey, new StoredInstanceKey(entry, ChangeTrackerActions.Delete), entry.GetChangedProperties());
        }


        public void Remove(CacheKey cacheKey, StoredInstanceKey storedInstanceKey, bool onlyFromTemp = false)
        {
            RemoveTempStoredKey(cacheKey, storedInstanceKey);
            if (!onlyFromTemp)
            {
                RemoveStoredKey(cacheKey, storedInstanceKey);
            }
        }


        public void Clear(CacheKey cacheKey, bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        {
            if (clearTemp)
            {
                ClearTempStoredKeys(cacheKey, tempModelType, forceClear);
            }

            if (clearStored)
            {
                ClearStoredKeys(cacheKey);
            }
        }

        #endregion
    }
}