using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    public interface IUsersManagers : IDBObject
    {

        #region Properties

        int ManagerID { get; set; }

        int SubordinateID { get; set; }

        #endregion

    }
}