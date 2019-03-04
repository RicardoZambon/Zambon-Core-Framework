namespace Zambon.Core.Module.Services
{
    /// <summary>
    /// Interface to implements in methods responsible to the current active language.
    /// </summary>
    public interface ILanguageService
    {

        /// <summary>
        /// Change the language by specifying a new language.
        /// </summary>
        /// <param name="newLanguage">The code of the new language to display.</param>
        void ChangeLanguage(string newLanguage);

        /// <summary>
        /// Get the current active language.
        /// </summary>
        /// <returns>Returns the current active language code.</returns>
        string GetCurrentLanguage();

    }
}