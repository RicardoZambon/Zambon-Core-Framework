using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zambon.Core.WebModule.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}