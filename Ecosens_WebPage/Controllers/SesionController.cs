using Ecosens_WebPage.Models;
using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecosens_WebPage.Controllers
{
    public class SesionController : Controller
    {
        private readonly LoginService loginService;

        public SesionController(LoginService loginService)
        {
            this.loginService = loginService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var (IsSuccess, Token, ErrorMessage) = await loginService.GetToken(model.Correo, model.Contraseña);

            return RedirectToAction("Index","Dashboard");

        }
    }
}
