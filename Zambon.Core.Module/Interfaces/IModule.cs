using System.Collections.Generic;

namespace Zambon.Core.Module.Interfaces
{
    public interface IModule
    {
        string ApplicationModelName { get; }

        IList<IModule> LoadModules();
    }
}