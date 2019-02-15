using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zambon.Core.Database.Entity
{
    public interface IDBObject : IEntity
    {

        int ID { get; set; }

        List<KeyValuePair<string, string>> ValidateData(CoreContext ctx);

    }
}