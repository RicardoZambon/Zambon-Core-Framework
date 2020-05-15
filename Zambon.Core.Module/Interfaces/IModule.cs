using System;
using System.Collections.Generic;

namespace Zambon.Core.Module.Interfaces
{
    public interface IModule
    {
        string ApplicationModelName { get; }

        Type ApplicationModelType { get; }

        IList<IModule> LoadModules();
    }
}