using Microsoft.AspNetCore.Http;
using Zambon.Core.Module.Services;

namespace Zambon.Core.WebModule.Services
{
    public class LanguageProvider : ILanguageProvider
    {
        private readonly IHttpContextAccessor HttpContextAcessor;

        protected string CurrentLanguage { get; private set; }

        public LanguageProvider(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAcessor = httpContextAccessor;
        }


        public void ChangeLanguage(string newLanguage)
        {
            CurrentLanguage = newLanguage;
            HttpContextAcessor.HttpContext.Response.Cookies.Append("CurrentLanguage", newLanguage);
        }

        public string GetCurrentLanguage()
        {
            if (!string.IsNullOrEmpty(CurrentLanguage))
            {
                return CurrentLanguage;
            }

            if (HttpContextAcessor.HttpContext.Request.Cookies.ContainsKey("CurrentLanguage"))
            {
                return HttpContextAcessor.HttpContext.Request.Cookies["CurrentLanguage"].ToString();
            }
            return string.Empty;
        }
    }
}