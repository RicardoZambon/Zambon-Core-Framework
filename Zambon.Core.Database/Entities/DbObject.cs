using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Domain.Interfaces;

namespace Zambon.Core.Database.Entities
{
    /// <summary>
    /// Represents database tables with primary key and soft delete column.
    /// </summary>
    public abstract class DbObject : BaseDBObject, ISoftDelete
    {
        /// <summary>
        /// Determines when the object is deleted and should be hidden in application.
        /// </summary>
        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }


        /// <summary>
        /// Called when executing OnConfiguring from CoreContext.
        /// </summary>
        /// <param name="entityBuilder">The object that can be used to configure a given entity type in the model.</param>
        public override void ConfigureEntity(EntityTypeBuilder entityBuilder)
        {
            base.ConfigureEntity(entityBuilder);
            if (entityBuilder.Metadata.GetRootType().ClrType.FullName == entityBuilder.Metadata.ClrType.FullName)
            {
                entityBuilder.HasQueryFilter(System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(GetType(), typeof(bool), "!IsDeleted", this));
            }
            entityBuilder.Property("IsDeleted").HasDefaultValueSql("0");
        }
    }
}