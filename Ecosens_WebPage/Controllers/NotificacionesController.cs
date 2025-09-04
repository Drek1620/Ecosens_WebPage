using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Ecosens_WebPage.Controllers
{
    public class NotificacionesController : Controller
    {
        private readonly NotificacionService notificacionService;
        private readonly SesionDataService sesionDataService;
        private readonly string _token;

        public NotificacionesController(NotificacionService notificacionService, SesionDataService sesionDataService)
        {
            this.notificacionService = notificacionService;
            this.sesionDataService = sesionDataService;
            _token = sesionDataService.GetToken();
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId");

            var ConsultaNotificaciones = await notificacionService.ObtenerNotificaciones(
                int.Parse(userId.Value), _token);

            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), _token);

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Sesion"); // Redirige al login 
            }

            ViewData["NombreUsuario"] = ConsultaDatosSesion.Nombre;
            ViewData["AreaId"] = ConsultaDatosSesion.AreaId;
            ViewData["Foto"] = ConsultaDatosSesion.Foto == "" ? null : ConsultaDatosSesion.Foto;
            ViewData["Notificacion"] = ConsultaDatosSesion.Notificaciones;

            return View(ConsultaNotificaciones);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarLeido(int id)
        {
            var ConsultaNotificaciones = await notificacionService.MarcarLeido(
                id, _token);
            return ConsultaNotificaciones ? Ok() : StatusCode(500);
        }
    }
}
