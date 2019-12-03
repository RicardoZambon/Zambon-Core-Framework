using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.ChangeTracker.Keys;
using Zambon.Core.Database.ChangeTracker.Services;

namespace Zambon.Core.Database.ChangeTracker
{
    /// <summary>
    /// Custom change tracker used to track instance property changes of all objects before actually saving in database.
    /// </summary>
    public class CoreChangeTracker
    {
        private const string TempPrefix = "TEMP-";

        #region Services

        private readonly IDistributedCache _distributedCache;

        private readonly IUserKeyProvider _userKeyProvider;

        private readonly IFormKeyProvider _formKeyProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="cache">Service to access the distributed cache.</param>
        /// <param name="userKeyProvider">Custom service to access the instance key stored at user presentation layer.</param>
        /// <param name="formKeyProvider">Custom service to access the current form key stored at current request.</param>
        // <param name="_changeTrackerOptions">Options used to configure the change tracker cache table.</param>
        public CoreChangeTracker(IDistributedCache cache, IUserKeyProvider userKeyProvider, IFormKeyProvider formKeyProvider)//, IOptions<SqlServerCacheOptions> changeTrackerOptions)
        {
            _distributedCache = cache;
            _userKeyProvider = userKeyProvider;
            _formKeyProvider = formKeyProvider;

            //using (var connection = new SqlConnection(_changeTrackerOptions.Value.ConnectionString))
            //{
            //    connection.Open();

            //    var schemaCommand = new SqlCommand($"IF NOT EXISTS(SELECT * FROM sys.schemas WHERE name = '{_changeTrackerOptions.Value.SchemaName}') BEGIN EXEC('CREATE SCHEMA {_changeTrackerOptions.Value.SchemaName}') END", connection);
            //    schemaCommand.ExecuteNonQuery();

            //    var tableCommand = new SqlCommand($"" +
            //        $"IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{_changeTrackerOptions.Value.SchemaName}' AND TABLE_NAME = '{_changeTrackerOptions.Value.TableName}')" +
            //        "BEGIN" +
            //        $"   CREATE TABLE {_changeTrackerOptions.Value.SchemaName}.{_changeTrackerOptions.Value.TableName}(" +
            //        "       [Id][nvarchar](449) NOT NULL," +
            //        "       [Value][varbinary](max) NOT NULL," +
            //        "       [ExpiresAtTime][datetimeoffset](7) NOT NULL," +
            //        "       [SlidingExpirationInSeconds][bigint] NULL," +
            //        "       [AbsoluteExpiration][datetimeoffset](7) NULL," +
            //        "   CONSTRAINT[PK_CachedData] PRIMARY KEY CLUSTERED" +
            //        "   (" +
            //        "   [Id] ASC" +
            //        "   )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]" +
            //        "   ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]" +
            //        "END", connection);
            //    tableCommand.ExecuteNonQuery();
            //}
        }

        #endregion


        private string GetStorageKey()
         => $"{_formKeyProvider.RetrieveFormKey().ToString()}@{_userKeyProvider.RetrieveKey().ToString()}";

        private string GetCacheKey(EntityInstanceKey instanceKey)
            => $"{instanceKey.ToString()}@{GetStorageKey()}";

        #region Read

        private EntityInstanceKey[] ReadTempInstanceKeys()
            => _distributedCache.Get($"{TempPrefix}{GetStorageKey()}").DeserializeEntityInstanceKeys();

        private EntityInstanceKey[] ReadStoredInstanceKeys()
            => _distributedCache.Get(GetStorageKey()).DeserializeEntityInstanceKeys();


        /// <summary>
        /// Checks if the change tracker is already tracking the object instance.
        /// </summary>
        /// <param name="instanceKey">The unique object instance key.</param>
        /// <param name="checkTemp">By default will only check object with already applied values.</param>
        /// <returns>Return true or false if the entity is being tracked.</returns>
        public bool IsTracking(EntityInstanceKey instanceKey, bool checkTemp = false)
            => (checkTemp && Array.IndexOf(ReadTempInstanceKeys(), instanceKey) > 0) || Array.IndexOf(ReadStoredInstanceKeys(), instanceKey) > 0;

        /// <summary>
        /// Checks if the change tracker is already tracking the object instance.
        /// </summary>
        /// <param name="dbContext">The database context instance.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="keyValues">Array of key values.</param>
        /// <param name="checkTemp">By default will only check object with already applied values.</param>
        /// <returns>Return true or false if the entity is being tracked.</returns>
        public bool IsTracking(DbContext dbContext, Type entityType, object[] keyValues, bool checkTemp = false)
            => IsTracking(new EntityInstanceKey(dbContext, entityType, keyValues), checkTemp);


        /// <summary>
        /// Load and return all objects of a specific entity type being currently tracked.
        /// </summary>
        /// <typeparam name="T">The type of the entity type.</typeparam>
        /// <param name="dbContext">The database instance context.</param>
        /// <param name="formKey">The parent form key, used to separate same user and different opened pages/forms.</param>
        /// <param name="readTemp">Should read and load objects from temp store.</param>
        /// <returns>Return an IQueryable list of the objects.</returns>
        public IQueryable<T> GetTrackedEntities<T>(DbContext dbContext, bool readTemp) where T : class
        {
            var loadedObjects = new List<T>();

            var entityTypeName = dbContext.Model.FindEntityType(typeof(T)).Name;

            var tempKeys = new EntityInstanceKey[0];
            if (readTemp)
            {
                tempKeys = ReadTempInstanceKeys();
                for (var t = 0; t < tempKeys.Length; t++)
                {
                    if (entityTypeName.Equals(tempKeys[t].EntityName))
                    {
                        loadedObjects.Add(
                            dbContext.LoadProperties<T>(tempKeys[t].EntityKeys,
                                _distributedCache.Get($"{TempPrefix}{GetCacheKey(tempKeys[t])}").DeserializeEntityInstanceObject())
                        );
                    }
                }
            }

            var storedKeys = ReadStoredInstanceKeys();
            for (var k = 0; k < storedKeys.Length; k++)
            {
                if (!tempKeys.Contains(storedKeys[k]) && entityTypeName.Equals(storedKeys[k].EntityName))
                {
                    loadedObjects.Add(
                        dbContext.LoadProperties<T>(storedKeys[k].EntityKeys,
                            _distributedCache.Get(GetCacheKey(storedKeys[k])).DeserializeEntityInstanceObject())
                    );
                }
            }

            return loadedObjects.AsQueryable();
        }



        #endregion

        #region Write

        private void WriteTempInstanceKeys(EntityInstanceKey[] keys)
            => _distributedCache.Set($"{TempPrefix}{GetStorageKey()}", keys.SerializeEntityInstanceKeys());

        private void WriteStoredInstanceKeys(EntityInstanceKey[] keys)
            => _distributedCache.Set(GetStorageKey(), keys.SerializeEntityInstanceKeys());


        /// <summary>
        /// Add or update an object into the ChangeTracker.
        /// </summary>
        /// <typeparam name="T">Must be an ITrackableEntity</typeparam>
        /// <param name="entry">The object instance.</param>
        /// <param name="saveToTemp">Should save in temporary edited objects or already applied changes.</param>
        public void AddOrUpdate<T>(EntityEntry<T> entry, bool saveToTemp = false) where T : class, ITrackableEntity
        {
            var instanceKey = new EntityInstanceKey(entry);
            var cacheKey = GetCacheKey(instanceKey);

            var tempInstanceKeys = ReadTempInstanceKeys().ToList();
            var storedInstanceKeys = ReadStoredInstanceKeys().ToList();

            var changedProperties = entry.GetChangedProperties();
            if ((changedProperties?.Count() ?? 0) > 0)
            {
                if (saveToTemp)
                {
                    if (!tempInstanceKeys.Contains(instanceKey))
                    {
                        tempInstanceKeys.Add(instanceKey);
                        WriteTempInstanceKeys(tempInstanceKeys.ToArray());
                    }
                    _distributedCache.Set($"{TempPrefix}{cacheKey}", changedProperties.SerializeEntityInstanceObject());
                }
                else
                {
                    if (tempInstanceKeys.Contains(instanceKey))
                    {
                        tempInstanceKeys.Remove(instanceKey);
                        WriteTempInstanceKeys(tempInstanceKeys.ToArray());
                        _distributedCache.Remove($"{TempPrefix}{cacheKey}");
                    }

                    if (!storedInstanceKeys.Contains(instanceKey))
                    {
                        storedInstanceKeys.Add(instanceKey);
                        WriteStoredInstanceKeys(storedInstanceKeys.ToArray());
                    }
                    _distributedCache.Set(cacheKey, changedProperties.SerializeEntityInstanceObject());
                }
            }
            else
            {
                if (tempInstanceKeys.Contains(instanceKey))
                {
                    _distributedCache.Remove($"{TempPrefix}{cacheKey}");
                }
                if (storedInstanceKeys.Contains(instanceKey))
                {
                    _distributedCache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Removes an object from ChangeTracker
        /// </summary>
        /// <param name="entry">The object instance.</param>
        /// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        public void Remove(EntityEntry entry, bool onlyFromTemp = false)
            => Remove(new EntityInstanceKey(entry), onlyFromTemp);

        /// <summary>
        /// Removes an object from ChangeTracker
        /// </summary>
        /// <param name="instanceKey">The object key.</param>
        /// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        public void Remove(EntityInstanceKey instanceKey, bool onlyFromTemp = false)
        {
            var cacheKey = GetCacheKey(instanceKey);

            var tempInstanceKeys = ReadTempInstanceKeys().ToList();
            if (tempInstanceKeys.Contains(instanceKey))
            {
                tempInstanceKeys.Remove(instanceKey);
                WriteTempInstanceKeys(tempInstanceKeys.ToArray());
                _distributedCache.Remove($"{TempPrefix}{cacheKey}");
            }

            if (!onlyFromTemp)
            {
                var storedInstanceKeys = ReadStoredInstanceKeys().ToList();
                if (storedInstanceKeys.Contains(instanceKey))
                {
                    storedInstanceKeys.Remove(instanceKey);
                    WriteStoredInstanceKeys(storedInstanceKeys.ToArray());
                    _distributedCache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Clears the stored entities in distributed cache.
        /// </summary>
        /// <param name="clearStored">If should clear the stored entities with changes already applied.</param>
        /// <param name="clearTemp">If should clear the temp stored entities.</param>
        /// <param name="tempEntityNameFilter">Filters the temp model to clean only this same type.</param>
        public void Clear(bool clearStored = true, bool clearTemp = true, string tempEntityNameFilter = "")
        {
            if (clearTemp)
            {
                var remainingKeys = new List<EntityInstanceKey>();

                var storedTempKeys = ReadTempInstanceKeys();
                for (var t = 0; t < storedTempKeys.Length; t++)
                {
                    if (!string.IsNullOrEmpty(tempEntityNameFilter) && tempEntityNameFilter.Equals(storedTempKeys[t].EntityName))
                    {
                        _distributedCache.Remove($"{TempPrefix}{GetCacheKey(storedTempKeys[t])}");
                    }
                    else
                    {
                        remainingKeys.Add(storedTempKeys[t]);
                    }
                }
                WriteTempInstanceKeys(remainingKeys.ToArray());
            }

            if (clearStored)
            {
                var storedKeys = ReadStoredInstanceKeys();
                for (var k = 0; k < storedKeys.Length; k++)
                {
                    _distributedCache.Remove(GetCacheKey(storedKeys[k]));
                }
                WriteStoredInstanceKeys(new EntityInstanceKey[0]);
            }
        }

        #endregion
    }
}