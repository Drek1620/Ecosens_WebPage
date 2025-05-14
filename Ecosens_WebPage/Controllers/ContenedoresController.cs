using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecosens_WebPage.Controllers
{
    public class ContenedoresController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
