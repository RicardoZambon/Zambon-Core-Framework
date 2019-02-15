using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;
using Zambon.Core.Module.Helper;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRolesPermissions : ITrackableEntity
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