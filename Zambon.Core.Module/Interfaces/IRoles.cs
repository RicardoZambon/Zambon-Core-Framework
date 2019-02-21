using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;
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