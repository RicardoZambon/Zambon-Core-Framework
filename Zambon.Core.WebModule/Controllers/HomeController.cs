using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.ActionFilters;
using Zambon.Core.WebModule.Models;

namespace Zambon.Core.WebModule.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationService _app;

        public HomeController(ApplicationService app)
        {
            _app = app;
        }

        [GenerateInstanceKey]
        public IActionResult Index()
        {
            var model = new IndexModel()
            {
                Title01 = string.Format(_app.GetStaticText("Home_SuperTitle"), _app.CurrentUser?.FullName),
                Title02 = string.Format(_app.GetStaticText("Home_Title"), _app.GetAppName()),
                Title03 = string.Format(_app.GetStaticText("Home_SubTitle"), _app.GetAppFullName()),

                Panel01_Title = _app.GetStaticText("Home_Panel1_Title"),
                Panel01_IconName = _app.GetStaticText("Home_Panel1_IconName"),
                Panel01_IconColor = _app.GetStaticText("Home_Panel1_IconColor"),
                Panel01_IconStyle = _app.GetStaticText("Home_Panel1_IconStyle"),

                Panel02_Title = _app.GetStaticText("Home_Panel2_Title"),
                Panel02_IconName = _app.GetStaticText("Home_Panel2_IconName"),
                Panel02_IconColor = _app.GetStaticText("Home_Panel2_IconColor"),
                Panel02_IconStyle = _app.GetStaticText("Home_Panel2_IconStyle"),

                Panel03_Title = _app.GetStaticText("Home_Panel3_Title"),
                Panel03_IconName = _app.GetStaticText("Home_Panel3_IconName"),
                Panel03_IconColor = _app.GetStaticText("Home_Panel3_IconColor"),
                Panel03_IconStyle = _app.GetStaticText("Home_Panel3_IconStyle"),
            };

            return View("Index", model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}