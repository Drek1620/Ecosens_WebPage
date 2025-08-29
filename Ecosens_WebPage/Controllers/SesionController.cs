using Ecosens_WebPage.Models;
using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecosens_WebPage.Controllers
{
    public class SesionController : Controller
    {
        private readonly LoginService loginService;
        private readonly SesionDataService sesionDataService;

        public SesionController(LoginService loginService, SesionDataService sesionDataService)
        {
            this.loginService = loginService;
            this.sesionDataService = sesionDataService;
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
            var ConsultaToken = await loginService.ObtenerToken(model.Correo, model.Contraseña);

            if (!ConsultaToken.IsSuccess || string.IsNullOrEmpty(ConsultaToken.Token))
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model); // Regresa al formulario de login con el mensaje de error
            }


            var claims = new List<Claim>
            {
                new Claim("UserId", ConsultaToken.UserId.ToString()),
                new Claim("TipoId", ConsultaToken.TipoId.ToString()),
                new Claim("AuthToken", ConsultaToken.Token) // Guardar token dentro de los claims si lo necesitas
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            
            await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               principal,
               new AuthenticationProperties
               {
                   IsPersistent = model.Recuerdame, // "Recordar sesión"
                   ExpiresUtc = DateTime.UtcNow.AddHours(1)
               });





            return RedirectToAction("Index", "Dashboard");

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Sesion"); // Redirige al login 
        }
    }
}
