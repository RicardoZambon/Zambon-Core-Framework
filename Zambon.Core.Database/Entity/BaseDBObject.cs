using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Database.Entity
{
    public abstract class BaseDBObject : IDBObject, IValidatableObject, ITrackableEntity
    {

        #region Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        #endregion

        #region Constructors

        public BaseDBObject()
        {

        }

        #endregion

        #region Validation

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new ValidationResult[] { };
        }

        public virtual List<KeyValuePair<string, string>> ValidateData(CoreContext ctx)
        {
            return new List<KeyValuePair<string, string>>();
        }

        #endregion

        #region Methods

        public virtual void ConfigureEntity(EntityTypeBuilder entity)
        {
            entity.Property<int>("ID").UseSqlServerIdentityColumn();

            entity.Property<int>("ID").ValueGeneratedOnAdd();
            entity.Property<int>("ID").Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
        }

        //public virtual void ConfigureDatabase(ModelBuilder modelBuilder)
        //{

        //}

        public virtual void OnSaving(CoreContext ctx)
        {

        }
        
        #endregion

    }
}