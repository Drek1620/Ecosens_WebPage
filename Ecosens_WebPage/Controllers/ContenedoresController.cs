using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecosens_WebPage.Controllers
{
    public class ContenedoresController : Controller
    {
        private readonly SesionDataService sesionDataService;

        public ContenedoresController(SesionDataService sesionDataService)
        {
            this.sesionDataService = sesionDataService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId");
            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), Request.Cookies["AuthToken"].ToString());

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Sesion"); // Redirige al login 
            }

            ViewData["NombreUsuario"] = ConsultaDatosSesion.Nombre;
            ViewData["AreaId"] = ConsultaDatosSesion.AreaId;
            ViewData["Foto"] = ConsultaDatosSesion.Foto == "" ? null : ConsultaDatosSesion.Foto;
            ViewData["Notificacion"] = ConsultaDatosSesion.Notificaciones;
            return View();
        }
    }
}
