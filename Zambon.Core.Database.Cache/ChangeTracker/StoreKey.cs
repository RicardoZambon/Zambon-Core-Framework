using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Zambon.Core.Database.Cache.ChangeTracker
{
    [Serializable]
    public class StoreKey
    {

        public string ModelType { get; private set; }

        public int EntityId { get; private set; }


        public StoreKey(string modelType, int entityId)
        {
            ModelType = modelType;
            EntityId = entityId;
        }


        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var compare = (StoreKey)obj;
            return ModelType.Equals(compare.ModelType) && EntityId.Equals(compare.EntityId);
        }

        public override string ToString()
        {
            return $"{ModelType}_{EntityId.ToString()}";
        }

    }
}