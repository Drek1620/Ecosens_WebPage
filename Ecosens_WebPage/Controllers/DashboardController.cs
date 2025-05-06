using Microsoft.AspNetCore.Mvc;

namespace Ecosens_WebPage.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
