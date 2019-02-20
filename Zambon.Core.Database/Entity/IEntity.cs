using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zambon.Core.Database.Entity
{
    public interface IEntity
    {

        void ConfigureEntity(EntityTypeBuilder entity);

    }
}