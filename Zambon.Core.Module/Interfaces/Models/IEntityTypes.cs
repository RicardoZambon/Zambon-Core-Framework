using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IEntityTypes<TEntity> where TEntity : BaseNode, IEntity
    {
        ChildItemCollection<TEntity> EntitiesList { get; set; }
    }
}
