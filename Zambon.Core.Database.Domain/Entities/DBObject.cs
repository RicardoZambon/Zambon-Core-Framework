using System.ComponentModel.DataAnnotations;

namespace Zambon.Core.Database.Domain.Entities
{
    /// <summary>
    /// Represents database tables with primary key and soft delete column.
    /// </summary>
    public abstract class DBObject : BaseDBObject
    {

        #region Properties

        /// <summary>
        /// Determines when the object is deleted and should be hidden in application.
        /// </summary>
        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }

        #endregion

        #region Overrides

        ///// <summary>
        ///// Called when executing OnConfiguring from CoreContext.
        ///// </summary>
        ///// <param name="entity">The object that can be used to configure a given entity type in the model.</param>
        //public override void ConfigureEntity(EntityTypeBuilder entity)
        //{
        //    base.ConfigureEntity(entity);

        //    if (entity.Metadata.RootType().ClrType.FullName == entity.Metadata.ClrType.FullName)
        //        entity.HasQueryFilter(System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(GetType(), typeof(bool), "!IsDeleted", this));

        //    entity.Property("IsDeleted").HasDefaultValueSql("0");
        //}

        #endregion

    }
}