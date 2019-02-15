using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Database.Cache.ChangeTracker
{
    public class InstanceKey
    {

        public Guid DatabaseKey { get; private set; }

        public int CurrentUserId { get; private set; }


        public InstanceKey(Guid databaseKey, int currentUserId)
        {
            DatabaseKey = databaseKey;
            CurrentUserId = currentUserId;
        }


        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var compare = (InstanceKey)obj;
            return DatabaseKey.Equals(compare.DatabaseKey) && CurrentUserId.Equals(compare.CurrentUserId);
        }

        public override string ToString()
        {
            return $"{DatabaseKey.ToString()}_{CurrentUserId.ToString()}";
        }

    }
}