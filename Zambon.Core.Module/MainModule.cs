using System;
using System.Collections.Generic;
using System.Linq;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module
{
    /// <summary>
    /// Module that makes the connection across Module, WebModule and Application.
    /// </summary>
    public class MainModule : IModule
    {
        #region Constants

        const string MODEL_NAME = "ApplicationModel";

        #endregion

        #region Properties

        /// <summary>
        /// The application model file name.
        /// </summary>
        public virtual string ApplicationModelName { get => MODEL_NAME; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all modules referenced externally the framework.
        /// </summary>
        /// <param name="modules">List of additional modules.</param>
        /// <returns></returns>
        public virtual List<Type> ReferencedModules(List<Type> modules)
        {
            if (modules == null)
            {
                throw new ArgumentNullException(nameof(modules));
            }
            return modules;
        }

        /// <summary>
        /// Load all modules.
        /// </summary>
        /// <returns></returns>
        public IList<IModule> LoadModules()
        {
            var loadedModules = new List<IModule>() { this };

            if (ReferencedModules(new List<Type>()) is List<Type> modules && modules.Count > 0)
            {
                loadedModules.AddRange(
                    modules.Select(x => (IModule)x.GetConstructor(new Type[] { }).Invoke(new object[] { }))
                );
            }

            var currentModel = GetType();
            while (currentModel.BaseType != null && currentModel.BaseType != typeof(object))
            {
                currentModel = currentModel.BaseType;
                loadedModules.Add((IModule)currentModel.GetConstructor(new Type[] { }).Invoke(new object[] { }));
            }

            return loadedModules;
        }

        #endregion
    }
}