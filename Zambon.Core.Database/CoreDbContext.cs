using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Zambon.Core.Database.ChangeTracker;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Database.Services;

namespace Zambon.Core.Database
{
    /// <summary>
    /// A DbContext instance represents a session with the database and can be used to query and save instances of your entities. DbContext is a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public class CoreDbContext : DbContext
    {
        #region Services

        private readonly CoreChangeTracker _trackedEntities;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Microsoft.EntityFrameworkCore.DbContext class using the specified options. The Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder) method will still be called to allow further configuration of the options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public CoreDbContext(DbContextOptions options) : base(options)
        {
            try
            {
                _trackedEntities = this.GetService<CoreChangeTracker>();
            }
            catch { }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types exposed in Microsoft.EntityFrameworkCore.DbSet`1 properties on your derived context. The resulting model may be cached and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically define extension methods on this object that allow you to configure aspects of the model that are specific to a given database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = this.GetService<IDbContextOptions>();
            var migrationsAssemblyName = RelationalOptionsExtension.Extract(options).MigrationsAssembly;

            var assemblies = new List<string>() { migrationsAssemblyName };

            if (GetAdditionalAssembliesService() is IDbAdditionalAssemblies additionalAssemblies)
            {
                assemblies.AddRange(additionalAssemblies.ReferencedAssemblies);
            }

            foreach (var assemblyName in assemblies.Distinct())
            {
                try
                {
                    var assembly = Assembly.Load(assemblyName);

                    modelBuilder.EntitiesFromAssembly(assembly);
                    modelBuilder.QueriesFromAssembly(assembly);
                    modelBuilder.ApplyConfigurationsFromAssembly(assembly);

                    foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                    {
                        relationship.DeleteBehavior = DeleteBehavior.Restrict;
                    }

                    modelBuilder.InitializeFromAssembly(assembly);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Console.WriteLine($"Could not load the file or assembly \"{assemblyName}\" when creating the database model.");
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the service instance for the additional assemblies list.
        /// </summary>
        /// <returns>Return the service interface with the additional assemblies. Null if not found any service registered.</returns>
        protected virtual IDbAdditionalAssemblies GetAdditionalAssembliesService()
        {
            return this.GetService<DbAdditionalAssemblies<CoreDbContext>>();
        }

        #endregion

        #region Change Tracker

        /// <summary>
        /// Returns all tracked entities of a specific type.
        /// </summary>
        /// <typeparam name="T">The entity type to get the tracked entities.</typeparam>
        /// <param name="readTemp">If should consider the temp store when loading the entities.</param>
        /// <returns>Returns all tracked entities of a specific type.</returns>
        public IQueryable<T> GetTrackedEntities<T>(bool readTemp) where T : class
            => _trackedEntities.GetTrackedEntities<T>(this, readTemp);

        ///// <summary>
        ///// Load tracked entities already stored in distributed cache.
        ///// </summary>
        //public void LoadTrackedEntities<T>(Guid formKey) where T : class, ITrackableEntity
        //    => _trackedEntities.LoadTrackedObjects<T>(this, formKey);


        ///// <summary>
        ///// Clears the stored entities in distributed cache.
        ///// </summary>
        ///// <param name="clearStored">If should clear the stored entities with changes already applyed.</param>
        ///// <param name="clearTemp">If should clear the temp stored entities.</param>
        ///// <param name="tempModelType">Filters the temp model to clean only this same type.</param>
        ///// <param name="forceClear">Force to clean all entities in temp.</param>
        //public void ClearTrackedEntities(bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        //    => TrackedEntities.Clear(clearStored, clearTemp, tempModelType, forceClear);


        /// <summary>
        /// Checks if the change tracker is already tracking the object instance.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="keys">The object entity keys.</param>
        /// <returns>If the object is already being tracked returns true.</returns>
        public bool IsTracking<T>(object[] keys)
            => _trackedEntities.IsTracking(this, typeof(T), keys, true);


        /// <summary>
        /// Applies the changed properties into distributed cache, without saving into database.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="SaveIntoTemp">If should save into temp or in tracked items.</param>
        public void ApplyChanges<T>(T entity, bool SaveIntoTemp = false) where T : class, ITrackableEntity
            => ApplyEntryChanges(Entry(entity), SaveIntoTemp);

        /// <summary>
        /// Applies the changed properties into distributed cache, without saving into database.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entry">The database entry instance.</param>
        /// <param name="SaveIntoTemp">If should save into temp or in tracked items.</param>
        public void ApplyEntryChanges<T>(EntityEntry<T> entry, bool SaveIntoTemp = false) where T : class, ITrackableEntity
        {
            if (entry.Entity is IKeyed baseDBEntity && baseDBEntity.ID == 0)
            {
                Add(entry.Entity);
            }
            _trackedEntities.AddOrUpdate(entry, SaveIntoTemp);
            if (!SaveIntoTemp)
            {
                _trackedEntities.Clear(clearStored: false, tempEntityNameFilter: entry.GetEntityType().Name);
            }
        }


        /// <summary>
        /// Remove the specified entity from the already tracked entities.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        public void RemoveTrackedEntity<T>(T entity, bool onlyFromTemp = false) where T : class
            => _trackedEntities.Remove(Entry(entity), onlyFromTemp);

        ///// <summary>
        ///// Retrieves the entity using the ID and remove it from the already tracked entities.
        ///// </summary>
        ///// <param name="formKey">The parent form key, used to separate same user and different opened pages/forms.</param>
        ///// <param name="entityType">The type of the entity.</param>
        ///// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        ///// <param name="keys">The entity keys to remove.</param>
        //public void RemoveTrackedEntity(Guid formKey, Type entityType, bool onlyFromTemp = false, params object[] keys)
        //    => _trackedEntities.Remove(formKey, new StoreKey(this, entityType, keys), onlyFromTemp);


        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity should be saved into the database.</param>
        /// <param name="UseTransaction">If set to true will use a transaction to save the entity into the database. Default true.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public int CommitChanges<T>(T entity, bool UseTransaction = true) where T : class
        {
            var useTransaction = (UseTransaction && Database.CurrentTransaction == null);
            IDbContextTransaction transaction = null;
            try
            {
                if (useTransaction)
                {
                    transaction = Database.BeginTransaction();
                }

                var dbEntries = base.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Deleted || x.State == EntityState.Modified).ToArray();
                foreach (var entry in dbEntries)
                {
                    entry.State = EntityState.Detached;
                }

                SaveObject(entity);
                var records = base.SaveChanges();

                if (useTransaction)
                {
                    transaction.Commit();
                }

                return records;
            }
            catch (Exception ex)
            {
                if (useTransaction)
                {
                    transaction.Rollback();
                }
                throw new Exception(ex.Message + (ex?.InnerException?.Message ?? string.Empty), ex);
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }

        private void SaveObject<T>(T entity) where T : class
        {
            EntityEntry<T> entry;

            if (entity is IDbObject dbObject)
            {
                dbObject.OnSaving(this);
            }

            if (this.IsNewEntry(entity))
            {
                entry = Add(entity);
                entry.State = EntityState.Added;
            }
            else
            {
                entry = Attach(entity);

                var modifiedCount = 0;
                var dbValues = entry.GetDatabaseValues();
                foreach (var property in dbValues.Properties)
                {
                    var dataTypes = property.PropertyInfo?.GetCustomAttributes(typeof(DataTypeAttribute), true);
                    if (dataTypes != null)
                    {
                        var isPassword = dataTypes.Count() > 0 && dataTypes.FirstOrDefault() is DataTypeAttribute dataType && dataType.DataType == DataType.Password;
                        if (property.Name != "Discriminator"
                            && ((isPassword && !string.IsNullOrWhiteSpace(entry.CurrentValues[property]?.ToString() ?? string.Empty)) || !isPassword)
                            && ((entry.CurrentValues[property] == null && dbValues[property] != null) || (entry.CurrentValues[property] != null && !entry.CurrentValues[property].Equals(dbValues[property]))))
                        {
                            entry.Property(property.Name).IsModified = true;
                            modifiedCount++;
                        }
                    }
                }

                if (modifiedCount == 0)
                {
                    entry.State = EntityState.Unchanged;
                }

                foreach (var nav in entry.Navigations)
                {
                    if (nav.CurrentValue is IEnumerable records)
                    {
                        typeof(CoreDbContext).GetMethod(nameof(SaveRelatedObjects)).MakeGenericMethod(records.GetType().GetGenericArguments()[0]).Invoke(this, null);
                    }
                }
            }
            _trackedEntities.Remove(entry);
        }

        /// <summary>
        /// Save related objects with the current parent object.
        /// </summary>
        /// <typeparam name="T">he entity type to search in change tracker.</typeparam>
        public void SaveRelatedObjects<T>() where T : class
        {
            var trackedEntities = _trackedEntities.GetTrackedEntities<T>(this, false);
            foreach (var record in trackedEntities)
            {
                SaveObject(record);
            }
        }

        #endregion
    }
}