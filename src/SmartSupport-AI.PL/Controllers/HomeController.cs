using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SmartSupport_AI.PL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
