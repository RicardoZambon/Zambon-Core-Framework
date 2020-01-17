using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using Zambon.Core.Database.ChangeTracker.Interfaces;

namespace Zambon.Core.Database.ChangeTracker.Services
{
    public class CoreChangeTrackerInstance
    {
        #region Services

        private readonly CoreChangeTrackerManager _changeTrackerManager;

        private readonly ICacheKeyService _cacheKeyService;

        #endregion

        #region Properties

        private bool LoadedEntities { get; set; }

        private CacheKey _cacheKey;
        public CacheKey CacheKey
        {
            get
            {
                if (_cacheKey == null)
                {
                    _cacheKey = new CacheKey(_cacheKeyService);
                }
                return _cacheKey;
            }
        }

        #endregion

        #region Constructors

        public CoreChangeTrackerInstance(CoreChangeTrackerManager changeTrackerManager, ICacheKeyService cacheKeyService)
        {
            _changeTrackerManager = changeTrackerManager;
            _cacheKeyService = cacheKeyService;
        }

        #endregion

        #region Methods - Read

        public void LoadAllTrackedInstances(DbContext dbContext)
        {
            if (!LoadedEntities)
            {
                LoadedEntities = true;
                _changeTrackerManager.LoadAllTrackedInstances(dbContext, CacheKey);
            }
        }

        public IEnumerable<T> GetTrackedInstances<T>(DbContext dbContext, bool readTemp) where T : class, ITrackableEntity
        {
            return _changeTrackerManager.GetTrackedInstances<T>(dbContext, CacheKey, readTemp);
        }

        [Obsolete]
        public bool IsTracking(StoredInstanceKey storedInstanceKey, bool checkTemp = false)
        {
            return _changeTrackerManager.IsTracking(CacheKey, storedInstanceKey, checkTemp);
        }

        #endregion

        #region Methods - Write

        public void AddOrUpdate<T>(EntityEntry<T> entry, bool saveToTemp = false) where T : class, ITrackableEntity
        {
            _changeTrackerManager.AddOrUpdate(CacheKey, entry, saveToTemp);
        }

        public void Delete<T>(EntityEntry<T> entry) where T : class, ITrackableEntity
        {
            _changeTrackerManager.Delete(CacheKey, entry);
        }


        public void Remove(StoredInstanceKey storedInstanceKey, bool onlyFromTemp = false)
        {
            _changeTrackerManager.Remove(CacheKey, storedInstanceKey, onlyFromTemp);
        }

        public void Remove(EntityEntry entry, bool onlyFromTemp = false)
        {
            _changeTrackerManager.Remove(CacheKey, new StoredInstanceKey(entry), onlyFromTemp);
        }


        public void Clear(bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        {
            _changeTrackerManager.Clear(CacheKey, clearStored, clearTemp, tempModelType, forceClear);
        }

        #endregion
    }
}