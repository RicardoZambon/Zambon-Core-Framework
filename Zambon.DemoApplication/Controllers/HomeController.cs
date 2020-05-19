using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.WebModule.Services;

namespace Zambon.DemoApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebModelProvider _modelProvider;

        public HomeController(IModelProvider modelProvider)
        {
            _modelProvider = (WebModelProvider)modelProvider;
        }

        public IActionResult Index()
        {
            var model = _modelProvider.GetModel("en");

            if (model != null)
            {

            }

            return View();
        }
    }
}
