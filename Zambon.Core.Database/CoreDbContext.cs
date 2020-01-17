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
using System.Threading;
using System.Threading.Tasks;
using Zambon.Core.Database.ChangeTracker.Extensions;
using Zambon.Core.Database.ChangeTracker.Interfaces;
using Zambon.Core.Database.ChangeTracker.Services;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Extensions;
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

        private readonly CoreChangeTrackerInstance _changeTrackerInstance;

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
                _changeTrackerInstance = this.GetService<CoreChangeTrackerInstance>();
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
                assemblies = assemblies.Distinct().ToList();
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

        #region Overrides - Change Tracker

        public override object Find(Type entityType, params object[] keyValues)
        {
            LoadAllTrackedEntities();
            return base.Find(entityType, keyValues);
        }
        public override TEntity Find<TEntity>(params object[] keyValues)
        {
            LoadAllTrackedEntities();
            return base.Find<TEntity>(keyValues);
        }

        public override async ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken)
        {
            LoadAllTrackedEntities();
            return base.FindAsync(entityType, keyValues, cancellationToken);
        }

        public override async ValueTask<object> FindAsync(Type entityType, params object[] keyValues)
        {
            LoadAllTrackedEntities();
            return base.FindAsync(entityType, keyValues);
        }

        public override async ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken)
        {
            LoadAllTrackedEntities();
            return await base.FindAsync<TEntity>(keyValues, cancellationToken);
        }

        public override async ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues)
        {
            LoadAllTrackedEntities();
            return await base.FindAsync<TEntity>(keyValues);
        }


        public override DbSet<TEntity> Set<TEntity>()
        {
            LoadAllTrackedEntities();
            return base.Set<TEntity>();
        }

        #endregion


        #region Methods

        /// <summary>
        /// Get the service instance for the additional assemblies list.
        /// </summary>
        /// <returns>Return the service interface with the additional assemblies. Null if not found any service registered.</returns>
        protected virtual IDbAdditionalAssemblies GetAdditionalAssembliesService()
            => this.GetService<DbAdditionalAssemblies<CoreDbContext>>();

        #endregion

        #region Methods - Change Tracker - Read

        private void LoadAllTrackedEntities()
        {
            if (_changeTrackerInstance != null)
            {
                _changeTrackerInstance.LoadAllTrackedInstances(this);
            }
        }


        /// <summary>
        /// Returns all tracked entities of a specific type.
        /// </summary>
        /// <typeparam name="T">The entity type to get the tracked entities.</typeparam>
        /// <param name="readTemp">If should consider the temp store when loading the entities.</param>
        /// <returns>Returns all tracked entities of a specific type.</returns>
        public IEnumerable<T> GetTrackedEntities<T>(bool readTemp) where T : class, ITrackableEntity
            => _changeTrackerInstance != null ? _changeTrackerInstance.GetTrackedInstances<T>(this, readTemp) : null;

        #endregion

        #region Methods - Change Tracker - Write

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
            if (_changeTrackerInstance != null)
            {
                _changeTrackerInstance.AddOrUpdate(entry, SaveIntoTemp);
                if (!SaveIntoTemp)
                {
                    _changeTrackerInstance.Clear(clearStored: false, tempModelType: entry.Entity.GetUnproxiedType().Name);
                }
            }
        }


        /// <summary>
        /// Remove the specified entity from the already tracked entities.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="onlyFromTemp">Removes only from temp store or from both.</param>
        public void RemoveTrackedEntity<T>(T entity, bool onlyFromTemp = false) where T : class
        {
            if (_changeTrackerInstance != null)
            {
                _changeTrackerInstance.Remove(Entry(entity), onlyFromTemp);
            }
        }


        public T Delete<T>(int id, bool _commitChanges = true) where T : class
        {
            if (Find<T>(id) is T entity)
            {
                if (entity is IDbObject dbObject)
                {
                    dbObject.OnDeleting(this);
                }

                if (entity is ITrackableEntity entityTrackable)
                {
                    if (this.IsNewEntry(entity))
                    {
                        RemoveTrackedEntity(entity);
                        return entity;
                    }

                    if (entity is ISoftDelete entityDBObject)
                    {
                        entityDBObject.IsDeleted = true;
                        if (_commitChanges)
                        {
                            CommitChanges(entity);
                        }
                        else
                        {
                            ApplyChanges(entityTrackable);
                        }
                    }
                    else
                    {
                        var entry = Remove(entityTrackable);
                        if (_commitChanges)
                        {
                            SaveChanges();
                        }
                        else
                        {
                            _changeTrackerInstance.Delete(entry);
                        }
                    }
                }
                else
                {
                    Remove(entity);
                    SaveChanges();
                }
                return entity;
            }
            throw new KeyNotFoundException();
        }


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
                    if (nav.CurrentValue is IEnumerable records && records.GetType().GetGenericArguments()[0].ImplementsInterface<ITrackableEntity>())
                    {
                        typeof(CoreDbContext).GetMethod(nameof(SaveRelatedObjects)).MakeGenericMethod(records.GetType().GetGenericArguments()[0]).Invoke(this, null);
                    }
                }
            }
            _changeTrackerInstance.Remove(entry);
        }

        /// <summary>
        /// Save related objects with the current parent object.
        /// </summary>
        /// <typeparam name="T">he entity type to search in change tracker.</typeparam>
        public void SaveRelatedObjects<T>() where T : class, ITrackableEntity
        {
            var trackedEntities = this.GetTrackedEntities<T>(false);
            foreach (var record in trackedEntities)
            {
                SaveObject(record);
            }
        }

        #endregion
    }
}