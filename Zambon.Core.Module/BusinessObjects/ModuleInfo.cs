using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Entity;

namespace Zambon.Core.Module.BusinessObjects
{
    public class ModuleInfo : BaseDBObject
    {

        #region Properties

        [StringLength(15)]
        public string DBVersion { get; set; }

        #endregion

        #region Overrides

        public override void ConfigureEntity(EntityTypeBuilder entity)
        {
            base.ConfigureEntity(entity);

            entity.Property("DBVersion").Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            entity.Property("DBVersion").Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
        }

        #endregion

    }
}
