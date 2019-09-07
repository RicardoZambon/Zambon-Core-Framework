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



        [Route("/lib/flagpack/flags/4x3/{*path}")]
        public IActionResult FlagPack_4x3(string path)
        {
            return Redirect("/lib/flagpack/flags/_4x3/"+path);
        }

        [Route("/lib/flagpack/flags/1x1/{*path}")]
        public IActionResult FlagPack_1x1(string path)
        {
            return Redirect("/lib/flagpack/flags/_1x1/" + path);
        }
    }
}