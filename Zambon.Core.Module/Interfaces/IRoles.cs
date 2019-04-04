using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interfaced used by roles.
    /// </summary>
    public interface IRoles : IDBObject
    {

        #region Properties

        /// <summary>
        /// The name of the role.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Indicates if the role is administrative (have access to all items).
        /// </summary>
        bool IsAdministrative { get; set; }

        #endregion

    }
}