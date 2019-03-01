using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.BusinessObjects
{
    public class RolesPermissions : DBObject, IRolesPermissions
    {

        #region Properties

        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Roles Role { get; set; }

        [MaxLength(100)]
        public string Entity { get; set; }

        [Display(Name = "Permission type")]
        public Module.Enums.PermissionTypes PermissionType { get; set; }

        public bool CanNavigate { get { return PermissionType.Equals(Module.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Enums.PermissionTypes.ReadOnly) || PermissionType.Equals(Module.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Enums.PermissionTypes.Navigate); } }
        public bool CanRead { get { return PermissionType.Equals(Module.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Enums.PermissionTypes.ReadOnly) || PermissionType.Equals(Module.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Enums.PermissionTypes.Read); } }
        public bool CanWrite { get { return PermissionType.Equals(Module.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Enums.PermissionTypes.Write); } }
        public bool CanCreate { get { return PermissionType.Equals(Module.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Enums.PermissionTypes.Create); } }
        public bool CanDelete { get { return PermissionType.Equals(Module.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Enums.PermissionTypes.Delete); } }

        #endregion

    }
}