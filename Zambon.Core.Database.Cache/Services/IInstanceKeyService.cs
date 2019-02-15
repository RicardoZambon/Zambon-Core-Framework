using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Database.Cache.ChangeTracker;

namespace Zambon.Core.Database.Cache.Services
{
    public interface IInstanceKeyService
    {

        void StoreKey(Guid databaseInstanceKey);

        InstanceKey RetrieveKey();

    }
}