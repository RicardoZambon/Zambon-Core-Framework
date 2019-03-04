using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used by roles permission control.
    /// </summary>
    public interface IRolesPermissions : IDBObject
    {

        #region Properties

        /// <summary>
        /// The role this permission access belongs.
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// The entity the permission should affect.
        /// </summary>
        string Entity { get; set; }

        /// <summary>
        /// The type of the permission.
        /// </summary>
        Enums.PermissionTypes PermissionType { get; set; }

        /// <summary>
        /// Indicates if the role will be able to navigate (menu).
        /// </summary>
        bool CanNavigate { get; }
        /// <summary>
        /// Indicates if the role will be able to read any record.
        /// </summary>
        bool CanRead { get; }
        /// <summary>
        /// Indicates if the role will be able to write (update/insert).
        /// </summary>
        bool CanWrite { get; }
        /// <summary>
        /// Indicates if the role will be able to create new records (insert).
        /// </summary>
        bool CanCreate { get; }
        /// <summary>
        /// Indicates if the role will be able to delete any record.
        /// </summary>
        bool CanDelete { get; }

        #endregion

    }
}