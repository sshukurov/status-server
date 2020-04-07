using Microsoft.AspNetCore.Mvc;

namespace Tedu.Server.Status.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
