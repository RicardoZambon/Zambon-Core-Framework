using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using System.Collections.Generic;

namespace Zambon.Core.Database.ChangeTracker.Services
{
    public class CoreChangeTrackerInstance
    {
        #region Services

        private readonly CoreChangeTrackerManager ChangeTrackerManager;

        private readonly ICacheKeyService CacheKeyService;

        #endregion

        #region Properties

        private CacheKey _cacheKey;
        public CacheKey CacheKey
        {
            get
            {
                if (_cacheKey == null)
                {
                    _cacheKey = new CacheKey(CacheKeyService);
                }
                return _cacheKey;
            }
        }

        #endregion

        #region Constructors

        public CoreChangeTrackerInstance(CoreChangeTrackerManager changeTrackerManager, ICacheKeyService cacheKeyService)
        {
            ChangeTrackerManager = changeTrackerManager;
            CacheKeyService = cacheKeyService;
        }

        #endregion

        #region Methods - Read

        public void LoadAllTrackedInstances(DbContext dbContext)
        {
            ChangeTrackerManager.LoadAllTrackedInstances(dbContext, CacheKey);
        }

        public IEnumerable<T> GetTrackedInstances<T>(DbContext dbContext, bool readTemp) where T : class, ITrackableEntity
        {
            return ChangeTrackerManager.GetTrackedInstances<T>(dbContext, CacheKey, readTemp);
        }

        public bool IsTracking(StoredInstanceKey storedInstanceKey, bool checkTemp = false)
        {
            return ChangeTrackerManager.IsTracking(CacheKey, storedInstanceKey, checkTemp);
        }

        #endregion

        #region Methods - Write

        public void Add<T>(EntityEntry<T> entry, bool saveToTemp = false) where T : class, ITrackableEntity
        {
            ChangeTrackerManager.Add(CacheKey, entry, saveToTemp);
        }

        public void Delete<T>(EntityEntry<T> entry) where T : class, ITrackableEntity
        {
            ChangeTrackerManager.Delete(CacheKey, entry);
        }


        public void Remove(StoredInstanceKey storedInstanceKey, bool onlyFromTemp = false)
        {
            ChangeTrackerManager.Remove(CacheKey, storedInstanceKey, onlyFromTemp);
        }


        public void Clear(bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        {
            ChangeTrackerManager.Clear(CacheKey, clearStored, clearTemp, tempModelType, forceClear);
        }

        #endregion
    }
}