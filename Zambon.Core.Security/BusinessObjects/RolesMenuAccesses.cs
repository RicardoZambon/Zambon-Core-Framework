using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.BusinessObjects
{
    public class RolesMenuAccesses : DBObject, IRolesMenuAccesses
    {

        #region Properties

        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Roles Role { get; set; }

        [Display(Name = "Menu ID")]
        public string MenuId { get; set; }

        [Display(Name = "Allow access?")]
        public bool AllowAccess { get; set; }

        #endregion

        #region Overrides

        public override void ConfigureEntity(EntityTypeBuilder entity)
        {
            base.ConfigureEntity(entity);

            entity.Property("AllowAccess").HasDefaultValueSql("0");
        }

        #endregion

    }
}