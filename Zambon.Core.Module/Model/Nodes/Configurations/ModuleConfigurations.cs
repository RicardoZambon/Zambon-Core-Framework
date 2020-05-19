using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Configurations
{
    public class ModuleConfigurations : SerializeNodeBase, IModuleConfigurations
    {
        #region Methods

        public virtual void Validate(IApplication applicationModel)
        {
        }

        #endregion
    }
}