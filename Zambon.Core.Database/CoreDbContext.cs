using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Zambon.Core.Database.ChangeTracker;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.Domain.Extensions;
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

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the Microsoft.EntityFrameworkCore.DbContext class using the specified options. The Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder) method will still be called to allow further configuration of the options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public CoreDbContext(DbContextOptions options) : base(options)
        {
            try
            {
                TrackedEntities = this.GetService<CoreChangeTracker>();
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
            var assembly = Assembly.Load(migrationsAssemblyName);

            //var configurationOptions = this.GetService<DbConfigurationOptions>();
            //foreach(var assemblyName in configurationOptions.ReferencedAssemblies)
            //{
            //    var assembly = Assembly.Load(assemblyName);

            modelBuilder.EntitiesFromAssembly(assembly);
            modelBuilder.QueriesFromAssembly(assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            var seedDatas = assembly.GetTypesByInterface<IDbInitializer>();
            if (seedDatas.Count() > 0)
            {
                foreach (var type in seedDatas)
                    if (!type.GetTypeInfo().IsAbstract && assembly.CreateInstance(type.FullName) is IDbInitializer dbInitializer)
                        dbInitializer.Seed(modelBuilder);
            }
            //}
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Change Tracker

        private readonly CoreChangeTracker TrackedEntities;


        /// <summary>
        /// Load tracked entities already stored in distributed cache.
        /// </summary>
        public void LoadTrackedEntities()
        {
            TrackedEntities.LoadObjects(this);
        }

        /// <summary>
        /// Clears the stored entities in distributed cache.
        /// </summary>
        /// <param name="clearStored">If should clear the stored entities with changes already applyed.</param>
        /// <param name="clearTemp">If should clear the temp stored entities.</param>
        /// <param name="tempModelType">Filters the temp model to clean only this same type.</param>
        /// <param name="forceClear">Force to clean all entities in temp.</param>
        public void ClearTrackedEntities(bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        {
            TrackedEntities.Clear(clearStored, clearTemp, tempModelType, forceClear);
        }


        /// <summary>
        /// Applies the changed properties into distributed cache, without saving into database.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="SaveIntoTemp">If should save into temp or in tracked items.</param>
        public void ApplyChanges<T>(T entity, bool SaveIntoTemp = false) where T : class, ITrackableEntity
        {
            var entry = Entry(entity);
            ApplyChanges(entry, SaveIntoTemp);
        }

        /// <summary>
        /// Applies the changed properties into distributed cache, without saving into database.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entry">The database entry instance.</param>
        /// <param name="SaveIntoTemp">If should save into temp or in tracked items.</param>
        public void ApplyChanges<T>(EntityEntry<T> entry, bool SaveIntoTemp = false) where T : class, ITrackableEntity
        {
            if (entry.Entity is IKeyed baseDBEntity && baseDBEntity.ID == 0)
                Add(entry.Entity);

            TrackedEntities.Add(entry, SaveIntoTemp);

            if (!SaveIntoTemp)
                TrackedEntities.Clear(clearStored: false, tempModelType: entry.Entity.GetUnproxiedType().Name);
        }


        /// <summary>
        /// Remove the specified entity from the already tracked entities.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        public void RemoveTrackedEntity<T>(T entity) where T : class
        {
            TrackedEntities.Remove(Entry(entity));
        }

        /// <summary>
        /// Retrieves the entity using the ID and remove it from the already tracked entities.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="keys">The entity keys to remove.</param>
        public void RemoveTrackedEntity(Type entityType, params object[] keys)
        {
            TrackedEntities.Remove(new StoreKey(entityType.Name, keys));
        }


        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity should be saved into the database.</param>
        /// <param name="UseTransaction">If set to true will use a transaction to save the entity into the database. Default true.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges<T>(T entity, bool UseTransaction = true) where T : class, IEntity
        {
            try
            {
                if (UseTransaction)
                    Database.BeginTransaction();

                var dbEntries = base.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Deleted || x.State == EntityState.Modified).ToArray();
                foreach (var entry in dbEntries)
                    entry.State = EntityState.Detached;

                SaveObject(entity);

                var records = base.SaveChanges();

                if (UseTransaction)
                    Database.CommitTransaction();

                TrackedEntities.Clear(forceClear: true);

                return records;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                    Database.RollbackTransaction();

                throw new Exception(ex.Message + ex?.InnerException?.Message, ex);
            }
        }

        private void SaveObject<T>(T entity) where T : class
        {
            if (this.IsNewEntry(entity))
            {
                var entry = Add(entity);
                entry.State = EntityState.Added;
            }
            else
            {
                var entry = Attach(entity);

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
                    entry.State = EntityState.Unchanged;

                foreach (var nav in entry.Navigations)
                    if (nav.CurrentValue is System.Collections.IEnumerable records)
                        foreach (var record in records)
                            if (record is ITrackableEntity trackableRecord && TrackedEntities.IsTracking(this, trackableRecord))
                                SaveObject(trackableRecord);
            }
        }

        #endregion

    }
}