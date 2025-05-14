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
            var (IsSuccess, Token,UserId,TipoId,Nombre, ErrorMessage) = await loginService.ObtenerToken(model.Correo, model.Contraseña);

            if (!IsSuccess || string.IsNullOrEmpty(Token))
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(model); // Regresa al formulario de login con el mensaje de error
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Nombre),
                new Claim("UserId", UserId.ToString()),
                new Claim("TipoId", TipoId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            Response.Cookies.Append("AuthToken", Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Solo se enviará en conexiones seguras
                SameSite = SameSiteMode.Strict, // Ayuda a prevenir CSRF
                Expires = DateTime.UtcNow.AddHours(1) // Establece la expiración
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



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
