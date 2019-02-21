using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Database.Cache.ChangeTracker
{
    public interface ITrackableEntity
    {

        /// <summary>
        /// Primary key of the entity
        /// </summary>
        int ID { get; set; }

    }
}