using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Module.Interfaces
{
    public interface IUsersManagers : ITrackableEntity
    {

        #region Properties

        int ManagerID { get; set; }

        int SubordinateID { get; set; }

        #endregion

    }
}