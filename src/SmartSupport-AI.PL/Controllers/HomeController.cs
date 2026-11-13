using Microsoft.AspNetCore.Mvc;
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
