using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.BusinessObjects
{
    public class UsersManagers : DBObject, IUsersManagers
    {

        #region Properties

        [Display(Name = "Manager")]
        public int ManagerID { get; set; }
        [JsonIgnore]
        public virtual Users Manager { get; set; }

        [Display(Name = "Subordinate")]
        public int SubordinateID { get; set; }
        [JsonIgnore]
        public virtual Users Subordinate { get; set; }

        #endregion
        
        #region Overrides

        public override void ConfigureEntity(EntityTypeBuilder entity)
        {
            base.ConfigureEntity(entity);

            entity.HasOne(typeof(Users), "Manager").WithMany("Subordinates").HasForeignKey("ManagerID");
            entity.HasOne(typeof(Users), "Subordinate").WithMany("Managers").HasForeignKey("SubordinateID");
        }

        #endregion

    }
}