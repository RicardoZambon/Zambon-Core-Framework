using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRolesMenuAccesses : ITrackableEntity
    {

        #region Properties

        int RoleId { get; set; }

        string MenuId { get; set; }

        bool AllowAccess { get; set; }

        #endregion

    }
}