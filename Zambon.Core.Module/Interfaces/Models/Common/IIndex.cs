using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Module.Interfaces.Models.Common
{
    public interface IIndex : IComparable
    {
        int? Index { get; set; }
    }
}
