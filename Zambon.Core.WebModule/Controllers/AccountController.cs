using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Zambon.Core.Module.Services;
using Zambon.Core.Security.Models;
using Zambon.Core.WebModule.ActionFilters;
using Zambon.Core.WebModule.CustomProviders;

namespace Zambon.Core.WebModule.Controllers
{
    public class AccountController : Controller
    {

        #region Variables

        private readonly IUserProvider UserProvider;
        private readonly ApplicationService ApplicationService;

        #endregion

        #region Constructors

        public AccountController(IUserProvider userProvider, ApplicationService applicationService)
        {
            UserProvider = userProvider;
            ApplicationService = applicationService;
        }

        #endregion

        #region Login

        [AllowAnonymous, Authorize()]
        public IActionResult Index(string returnUrl = "")
        {
            if (ApplicationService.CurrentUser != null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Login", new { returnUrl });
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (ApplicationService.CurrentUser != null)
                return RedirectToAction("Index", "Home");

            return View("Index", new Login() { ReturnUrl = returnUrl });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, ValidateActionOnSaving]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (UserProvider.LogonAllowed(model.Username))
                    {
                        var checkPassword = await UserProvider.CheckPasswordAsync(model.Username, model.Password);
                        if (checkPassword.Succeeded)
                        {
                            var principal = await UserProvider.CreatePrincipalAsync(model.Username);
                            await HttpContext.SignInAsync(principal, new AuthenticationProperties() { IsPersistent = model.RememberMe, ExpiresUtc = new DateTimeOffset(DateTime.Now.Date.AddDays(14)), IssuedUtc = new DateTimeOffset(DateTime.Now) });
                            return Content(string.Format("<script language='javascript' type='text/javascript'>window.location = '{0}';</script>", Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl : Url.Action("Index", "Home")));
                        }
                    }
                    ModelState.AddModelError("LoginError", ApplicationService.GetStaticText("Login_SignInError"));
                }
                catch (Exception ex) { ModelState.AddModelError("LoginError", string.Format(ApplicationService.GetStaticText("Error_Message"), ex.Message)); }
            }
            return PartialView("_LoginForm", model);            
        }

        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Url.Action("Index", "Home"));
        }

        #endregion

    }
}