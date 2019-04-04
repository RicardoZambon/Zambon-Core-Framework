using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used by menu access control.
    /// </summary>
    public interface IRolesMenuAccesses : IDBObject
    {

        #region Properties

        /// <summary>
        /// The role this menu access belongs.
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// The ID of the menu should allow/deny the access.
        /// </summary>
        string MenuId { get; set; }

        /// <summary>
        /// Indicates if the users should have access (true) or not (false) to the menu ID.
        /// </summary>
        bool AllowAccess { get; set; }

        #endregion

    }
}