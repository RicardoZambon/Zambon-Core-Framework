using Microsoft.AspNetCore.Mvc;
using Zambon.Core.Module.Services;
using Zambon.Core.WebModule.ActionFilters;
using Zambon.Core.WebModule.Models;

namespace Zambon.Core.WebModule.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationService ApplicationService;

        public HomeController(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }

        [GenerateInstanceKey]
        public IActionResult Index()
        {
            var model = new IndexModel()
            {
                Title01 = string.Format(ApplicationService.GetStaticText("Home_SuperTitle"), ApplicationService.CurrentUser?.FullName),
                Title02 = string.Format(ApplicationService.GetStaticText("Home_Title"), ApplicationService.AppName),
                Title03 = string.Format(ApplicationService.GetStaticText("Home_SubTitle"), ApplicationService.AppFullName),

                Panel01_Title = ApplicationService.GetStaticText("Home_Panel1_Title"),
                Panel01_IconName = ApplicationService.GetStaticText("Home_Panel1_IconName"),
                Panel01_IconColor = ApplicationService.GetStaticText("Home_Panel1_IconColor"),
                Panel01_IconStyle = ApplicationService.GetStaticText("Home_Panel1_IconStyle"),

                Panel02_Title = ApplicationService.GetStaticText("Home_Panel2_Title"),
                Panel02_IconName = ApplicationService.GetStaticText("Home_Panel2_IconName"),
                Panel02_IconColor = ApplicationService.GetStaticText("Home_Panel2_IconColor"),
                Panel02_IconStyle = ApplicationService.GetStaticText("Home_Panel2_IconStyle"),

                Panel03_Title = ApplicationService.GetStaticText("Home_Panel3_Title"),
                Panel03_IconName = ApplicationService.GetStaticText("Home_Panel3_IconName"),
                Panel03_IconColor = ApplicationService.GetStaticText("Home_Panel3_IconColor"),
                Panel03_IconStyle = ApplicationService.GetStaticText("Home_Panel3_IconStyle"),
            };

            return View("Index", model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}