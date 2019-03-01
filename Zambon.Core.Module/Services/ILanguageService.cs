namespace Zambon.Core.Module.Services
{
    public interface ILanguageService
    {

        void ChangeLanguage(string newLanguage);

        string GetCurrentLanguage();

    }
}