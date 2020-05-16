using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Module.Interfaces.Models
{
    public interface IView : IParent
    {
        string Id { get; set; }

        string Title { get; set; }

        string EntityId { get; set; }
    }
}