using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    public class ModelService<T> : IModelService where T : class, IModule
    {        
        private readonly T _mainModule;



        public ModelService(T mainModule)
        {
            this._mainModule = mainModule;
        }


        public void LoadModels()
        {
            var loadedModules = _mainModule.LoadModules();

            //Load languages


            var serializer = new XmlSerializer(typeof(Application));
            var model = SerializeModel(serializer, loadedModules);

            if (model == null)
            {
                throw new NullReferenceException();
            }
        }

        protected Application SerializeModel(XmlSerializer serializer, IList<IModule> modules, string language = null)
        {
            Application model = null;

            foreach(var module in modules)
            {
                var assembly = module.GetType().Assembly;
                if (!string.IsNullOrEmpty(language))
                {
                    assembly = assembly.GetSatelliteAssembly(CultureInfo.CreateSpecificCulture(language));
                }

                try
                {
                    using (var webModuleStream = assembly.GetManifestResourceStream($"{module.GetType().Assembly.GetName().Name}.{module.ApplicationModelName}.xml"))
                    {
                        if (webModuleStream != null && serializer.Deserialize(webModuleStream) is Application applicationModel)
                        {
                            if (model == null)
                            {
                                model = applicationModel;
                            }
                            else
                            {
                                //Merge
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    //Module is not using XML model, then, do nothing.
                }
            }

            return model;
        }
    }
}
