using Microsoft.AspNetCore.Http;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.WebModule.Services
{
    public class LanguageService : ILanguageService
    {

        private readonly IHttpContextAccessor HttpContextAcessor;

        public LanguageService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAcessor = httpContextAccessor;
        }


        public void ChangeLanguage(string newLanguage)
        {
            HttpContextAcessor.HttpContext.Response.Cookies.Append("CurrentLanguage", newLanguage);
        }

        public string GetCurrentLanguage()
        {
            if (HttpContextAcessor.HttpContext.Request.Cookies.ContainsKey("CurrentLanguage"))
                return HttpContextAcessor.HttpContext.Request.Cookies["CurrentLanguage"].ToString();
            return string.Empty;
        }

    }
}