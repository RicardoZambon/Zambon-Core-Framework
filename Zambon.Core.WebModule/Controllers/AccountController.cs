using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zambon.Core.Security;
using Zambon.Core.Security.Identity;
using Zambon.Core.Database;
using Zambon.Core.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Zambon.Core.Module.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;
using Zambon.Core.WebModule.CustomProviders;

namespace Zambon.Core.WebModule.Controllers
{
    public class AccountController : Controller
    {

        #region Variables

        private readonly IUserProvider _userProvider;
        private readonly ApplicationService _app;

        #endregion

        #region Constructors

        public AccountController(IUserProvider userProvider, ApplicationService app)
        {
            _userProvider = userProvider;
            _app = app;
        }

        #endregion

        #region Login

        [AllowAnonymous, Authorize()]
        public IActionResult Index()
        {
            if (_app.CurrentUser != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (_app.CurrentUser != null)
                return RedirectToAction("Index", "Home");

            return View("Index", new Login() { ReturnUrl = returnUrl ?? "" });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_userProvider.LogonAllowed(model.Username))
                    {
                        var checkPassword = await _userProvider.CheckPasswordAsync(model.Username, model.Password);
                        if (checkPassword.Succeeded)
                        {
                            var principal = await _userProvider.CreatePrincipalAsync(model.Username);
                            await HttpContext.SignInAsync(principal, new AuthenticationProperties() { IsPersistent = model.RememberMe, ExpiresUtc = new DateTimeOffset(DateTime.Now.Date.AddDays(14)), IssuedUtc = new DateTimeOffset(DateTime.Now) });
                            return Content(string.Format("<script language='javascript' type='text/javascript'>window.location = '{0}';</script>", (Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl : Url.Action("Index", "Home"))));
                        }
                    }
                    ModelState.AddModelError("LoginError", _app.GetStaticText("Login_SignInError"));
                }
                catch (Exception ex) { ModelState.AddModelError("LoginError", string.Format(_app.GetStaticText("Error_Message"), ex.Message)); }
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