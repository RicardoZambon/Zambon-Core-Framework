using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used to map user with managers and subordinates.
    /// </summary>
    public interface IUsersManagers : IDBObject
    {

        #region Properties

        /// <summary>
        /// The user ID.
        /// </summary>
        int ManagerID { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        int SubordinateID { get; set; }

        #endregion

    }
}