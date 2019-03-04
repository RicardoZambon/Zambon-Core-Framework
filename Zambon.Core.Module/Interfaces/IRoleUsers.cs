using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used to control the relation between roles and users.
    /// </summary>
    public interface IRolesUsers : IDBObject
    {

        #region Properties

        /// <summary>
        /// The role ID.
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        int UserId { get; set; }

        #endregion

    }
}