using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Validations;

namespace Zambon.Core.Security.BusinessObjects
{
    [DefaultProperty("Name")]
    public class Roles : DBObject, IRoles
    {

        #region Properties

        [Display(Name = "Role name"), StringLength(100)]
        [RuleRequired, RuleUniqueValue]
        public string Name { get; set; }

        [Display(Name = "Admins?")]
        public bool IsAdministrative { get; set; }

        #endregion

        #region Relationships

        [Display(Name = "Users in role")]
        public virtual ICollection<RolesUsers> Users { get; set; }

        [Display(Name = "Permissions in role")]
        public virtual ICollection<RolesPermissions> Permissions { get; set; }

        [Display(Name = "Menu access")]
        public virtual ICollection<RolesMenuAccesses> MenuAccess { get; set; }

        #endregion

        #region Overrides

        public override void Configure(EntityTypeBuilder entity)
        {
            base.Configure(entity);

            entity.Property("IsAdministrative").HasDefaultValueSql("0");
        }

        #endregion

    }
}