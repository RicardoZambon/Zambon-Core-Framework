using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;
using Zambon.Core.Database.Interfaces;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRolesMenuAccesses : IDBObject
    {

        #region Properties

        int RoleId { get; set; }

        string MenuId { get; set; }

        bool AllowAccess { get; set; }

        #endregion

    }
}