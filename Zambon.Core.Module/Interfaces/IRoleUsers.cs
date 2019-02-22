using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRolesUsers : IDBObject
    {

        #region Properties

        int RoleId { get; set; }

        int UserId { get; set; }

        #endregion

    }
}