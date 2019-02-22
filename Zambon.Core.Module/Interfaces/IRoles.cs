using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRoles : IDBObject
    {

        #region Properties

        string Name { get; set; }

        bool IsAdministrative { get; set; }

        #endregion

    }
}