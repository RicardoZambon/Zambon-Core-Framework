using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Module.Interfaces
{
    public interface IRolesUsers : ITrackableEntity
    {

        #region Properties

        int RoleId { get; set; }

        int UserId { get; set; }

        #endregion

    }
}