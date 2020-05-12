using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module
{
    public class MainModule : IModule
    {
        const string MODEL_NAME = "ApplicationModel";

        public virtual string ApplicationModelName { get => MODEL_NAME; }


        public MainModule()
        {
            
        }


        public virtual List<Type> ReferencedModels(List<Type> models) => models;

        public T CreateInstance<T>() where T : class, IModule, new() => new T();

        public IList<IModule> LoadModules()
        {
            var loadedModels = new List<IModule>() { this };

            if (ReferencedModels(new List<Type>()) is List<Type> modules && modules.Count > 0)
            {
                foreach (var model in modules)
                {
                    loadedModels.Add(
                        (IModule)model.GetConstructor(new Type[] { }).Invoke(new object[] { })
                    );
                }
            }

            var currentModel = GetType();
            while(currentModel.BaseType != null && currentModel.BaseType != typeof(System.Object))
            {
                loadedModels.Add((IModule)currentModel.BaseType.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                currentModel = currentModel.BaseType;
            }

            return loadedModels;
        }
    }
}