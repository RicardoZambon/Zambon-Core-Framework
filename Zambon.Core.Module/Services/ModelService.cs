using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    public class ModelService<T> : IModelService where T : class, IModule
    {
        private readonly IModule _mainModule;
        private readonly ModelStore _modelStore;

        public ModelService(IModule mainModule, ModelStore modelStore)
        {
            this._mainModule = mainModule;
            this._modelStore = modelStore;
        }


        public void GetModel(string language)
        {
            _modelStore.GetModel(_mainModule, language);
        }
    }
}