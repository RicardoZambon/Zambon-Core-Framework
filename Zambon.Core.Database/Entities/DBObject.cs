using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Zambon.Core.Database.Entity
{
    /// <summary>
    /// Represents database tables with primary key and soft delete column.
    /// </summary>
    public abstract class DBObject : BaseDBObject
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
        public override void Configure(EntityTypeBuilder entityBuilder)
        {
            base.Configure(entityBuilder);

            if (entityBuilder.Metadata.RootType().ClrType.FullName == entityBuilder.Metadata.ClrType.FullName)
                entityBuilder.HasQueryFilter(System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(GetType(), typeof(bool), "!IsDeleted", this));

            entityBuilder.Property("IsDeleted").HasDefaultValueSql("0");
        }
    }
}