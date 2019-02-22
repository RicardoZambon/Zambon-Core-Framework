using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Zambon.Core.Database.ChangeTracker;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.ExtensionMethods;
using Zambon.Core.Database.Interfaces;

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
            //Database.Migrate();

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

            foreach (var entity in assembly.GetReferencedClasses<IEntity>())
            {
                var modelBuilderEntity = modelBuilder.Entity(entity.GetType());
                entity.ConfigureEntity(modelBuilderEntity);
            }

            foreach (var query in assembly.GetReferencedClasses<IQuery>())
                modelBuilder.Query(query.GetType());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            var seedDatas = assembly.GetTypesByInterface<ICoreDbSeedData>();
            if (seedDatas.Count() > 0)
            {
                foreach (var type in seedDatas)
                    if (!type.GetTypeInfo().IsAbstract && assembly.CreateInstance(type.FullName) is ICoreDbSeedData seedData)
                        seedData.SeedData(modelBuilder);
            }

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
            if (entry.Entity is BaseDBObject baseDBEntity && baseDBEntity.ID == 0)
                Add(entry.Entity);

            TrackedEntities.Add( entry, SaveIntoTemp);

            if (!SaveIntoTemp)
                TrackedEntities.Clear(clearStored: false, tempModelType: entry.Entity.GetType().GetCorrectTypeName());
        }


        /// <summary>
        /// Remove the specified entity from the already tracked entities.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        public void RemoveTrackedEntity<T>(T entity) where T: class, ITrackableEntity
        {
            TrackedEntities.Remove(typeof(T).GetCorrectTypeName(), entity.ID);
        }

        /// <summary>
        /// Retrives the entity using the ID and remove it from the already tracked entities.
        /// </summary>
        /// <param name="modelType">Type of the entity.</param>
        /// <param name="entityId">The entity id.</param>
        public void RemoveTrackedEntity(Type modelType, int entityId) 
        {
            TrackedEntities.Remove(modelType.GetCorrectTypeName(), entityId);
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
                
                TrackedEntities.SaveObjects(this, entity);

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

        #endregion

        #region Methods

        /// <summary>
        /// Creates a Microsoft.EntityFrameworkCore.DbSet`1 that can be used to query and save instances of TEntity.
        /// </summary>
        /// <param name="_type">The type of entity for which a set should be returned.</param>
        /// <returns>A set for the given entity type.</returns>
        public IQueryable Set(Type _type)
        {
            return (IQueryable)typeof(DbContext).GetMethod("Set").MakeGenericMethod(_type).Invoke(this, null);
        }

        #endregion

    }
}