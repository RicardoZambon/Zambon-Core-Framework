using Microsoft.Extensions.Options;

namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// Service to get the core configuration used in application.
    /// </summary>
    public class CoreConfigsService
    {
        #region Services

        private readonly CoreConfigs CoreConfigs;

        #endregion

        #region Properties

        /// <summary>
        /// The current configuration used in application.
        /// </summary>
        public CoreConfigs Configs => CoreConfigs;

        #endregion

        #region Constructors

        /// <summary>
        /// Service to get the core configuration used in application.
        /// </summary>
        /// <param name="coreConfigs">The core configuration IOptions instance.</param>
        public CoreConfigsService(IOptions<CoreConfigs> coreConfigs)
        {
            CoreConfigs = coreConfigs.Value;
        }

        #endregion
    }
}