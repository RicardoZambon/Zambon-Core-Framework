using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// The main service used in application, is registered under Scoped life-cycle.
    /// </summary>
    public class ApplicationService
    {
        #region Services

        private readonly CoreConfigsService CoreConfigsService;

        private readonly ModelService ModelService;

        private readonly ILanguageProvider LanguageProvider;

        #endregion

        #region Properties


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for the ApplicationService.
        /// </summary>
        /// <param name="coreConfigsService">Instance of the AppSettings.json file.</param>
        /// <param name="languageProvider">Current language service.</param>
        /// <param name="modelService">ApplicationModel.xml service.</param>
        public ApplicationService(CoreConfigsService coreConfigsService, ModelService modelService, ILanguageProvider languageProvider)
        {
            CoreConfigsService = coreConfigsService;
            ModelService = modelService;
            LanguageProvider = languageProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the application title.
        /// </summary>
        /// <returns>Returns the application title.</returns>
        public string GetApplicationTitle()
        {
            var stringBuilder = new StringBuilder();

            if ((ModelService.GetConfiguration()?.TitleDefaults?.ShowEnvironment ?? false) && CoreConfigsService.Configs.HasEnvironmentTitle)
            {
                stringBuilder.Append($"[{CoreConfigsService.Configs.EnvironmentTitle}] ");
            }

            stringBuilder.Append(ModelService.AppName);

            if (ModelService.GetConfiguration()?.TitleDefaults?.ShowVersion ?? false)
            {
                stringBuilder.Append($" v{ModelService.Version}");
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}