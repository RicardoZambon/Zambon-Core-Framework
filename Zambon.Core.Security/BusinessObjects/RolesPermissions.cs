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
        public Module.Helper.Enums.PermissionTypes PermissionType { get; set; }

        public bool CanNavigate { get { return PermissionType.Equals(Module.Helper.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.ReadOnly) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.Navigate); } }
        public bool CanRead { get { return PermissionType.Equals(Module.Helper.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.ReadOnly) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.Read); } }
        public bool CanWrite { get { return PermissionType.Equals(Module.Helper.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.Write); } }
        public bool CanCreate { get { return PermissionType.Equals(Module.Helper.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.ReadWrite) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.Create); } }
        public bool CanDelete { get { return PermissionType.Equals(Module.Helper.Enums.PermissionTypes.FullAccess) || PermissionType.Equals(Module.Helper.Enums.PermissionTypes.Delete); } }

        #endregion

    }
}