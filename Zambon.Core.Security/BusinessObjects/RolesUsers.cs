using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.BusinessObjects
{
    public class RolesUsers : DBObject, IRolesUsers
    {

        #region Properties

        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Roles Role { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual Users User { get; set; }

        #endregion

    }
}