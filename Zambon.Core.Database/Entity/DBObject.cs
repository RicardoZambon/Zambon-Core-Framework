using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Zambon.Core.Database.Entity
{
    public abstract class DBObject : BaseDBObject
    {

        #region Properties

        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }

        #endregion

        #region Overrides

        public override void ConfigureEntity(EntityTypeBuilder entity)
        {
            base.ConfigureEntity(entity);

            if (entity.Metadata.RootType().ClrType.FullName == entity.Metadata.ClrType.FullName)
                entity.HasQueryFilter(System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(GetType(), typeof(bool), "!IsDeleted", this));

            entity.Property("IsDeleted").HasDefaultValueSql("0");
        }

        #endregion

    }
}