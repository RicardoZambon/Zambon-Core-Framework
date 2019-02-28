namespace Zambon.Core.Module.Interfaces
{
    public interface ILanguageService
    {

        void ChangeLanguage(string newLanguage);

        string GetCurrentLanguage();

    }
}