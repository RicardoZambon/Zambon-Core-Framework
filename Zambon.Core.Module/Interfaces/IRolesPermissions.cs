using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRolesPermissions : IDBObject
    {

        #region Properties

        int RoleId { get; set; }

        string Entity { get; set; }

        Enums.PermissionTypes PermissionType { get; set; }

        bool CanNavigate { get; }
        bool CanRead { get; }
        bool CanWrite { get; }
        bool CanCreate { get; }
        bool CanDelete { get; }

        #endregion

    }
}