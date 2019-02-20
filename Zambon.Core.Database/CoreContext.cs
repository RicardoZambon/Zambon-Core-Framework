using Zambon.Core.Database.Entity;
using Zambon.Core.Database.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using JetBrains.Annotations;
using System.Threading;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel;
using Zambon.Core.Database.Cache.ChangeTracker;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Zambon.Core.Database
{
    public class CoreContext : DbContext
    {

        #region Constructors

        public CoreContext(DbContextOptions options) : base(options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = this.GetService<IDbContextOptions>();
            var migrationsAssemblyName = RelationalOptionsExtension.Extract(options).MigrationsAssembly;
            var assembly = Assembly.Load(migrationsAssemblyName);

            foreach (var entity in AssemblyHelper.GetReferencedClasses<IEntity>(assembly))
            {
                var modelBuilderEntity = modelBuilder.Entity(entity.GetType());
                entity.ConfigureEntity(modelBuilderEntity);
            }

            foreach (var query in AssemblyHelper.GetReferencedClasses<IQuery>(assembly))
                modelBuilder.Query(query.GetType());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Change Tracker

        private readonly CoreChangeTracker TrackedEntities;


        public void LoadTrackedEntities()
        {
            TrackedEntities.LoadObjects(this);
        }

        public void ClearTrackedEntities(bool clearStored = true, bool clearTemp = true, string tempModelType = "", bool forceClear = false)
        {
            TrackedEntities.Clear(clearStored, clearTemp, tempModelType, forceClear);
        }


        public void ApplyChanges<T>(T entity, bool SaveIntoTemp = false) where T : class, ITrackableEntity
        {
            var entry = Entry(entity);
            ApplyChanges(entry, SaveIntoTemp);
        }

        public void ApplyChanges<T>(EntityEntry<T> entry, bool SaveIntoTemp = false) where T : class, ITrackableEntity
        {
            if (entry.Entity is BaseDBObject baseDBEntity && baseDBEntity.ID == 0)
                Add(entry.Entity);

            TrackedEntities.Add( entry, SaveIntoTemp);

            if (!SaveIntoTemp)
                TrackedEntities.Clear(clearStored: false, tempModelType: entry.Entity.GetType().GetCorrectTypeName());
        }


        public void RemoveTrackedEntity<T>(T entity) where T: class, ITrackableEntity
        {
            TrackedEntities.Remove(typeof(T).GetCorrectTypeName(), entity.ID);
        }

        public void RemoveTrackedEntity(Type modelType, int entityId) 
        {
            TrackedEntities.Remove(modelType.GetCorrectTypeName(), entityId);
        }


        public int SaveChanges<T>(T entity, bool UseTransaction = true) where T : class
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

        //protected virtual void DatabaseCreated()
        //{
        //    OnDatabaseCreated?.Invoke(this);
        //}

        public IQueryable Set(Type _type)
        {
            return (IQueryable)typeof(DbContext).GetMethod("Set").MakeGenericMethod(_type).Invoke(this, null);
        }

        #endregion

        //#region Events

        //public delegate void DatabaseEventHandler(CoreContext ctx);

        //public static event DatabaseEventHandler OnDatabaseCreated;

        //#endregion

    }
}