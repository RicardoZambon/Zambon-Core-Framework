﻿using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;
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