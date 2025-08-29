using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecosens_WebPage.Models;

namespace Ecosens_WebPage.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SesionDataService sesionDataService;
        private readonly DashboardService dashboardService;
        private readonly string _apiBaseUrl;

        public DashboardController(SesionDataService sesionDataService, DashboardService dashboardService, IConfiguration configuration)
        {
            this.sesionDataService = sesionDataService;
            this.dashboardService = dashboardService;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var userId = User.FindFirst("UserId");
            var TipoId = User.FindFirst("TipoId");

            ViewBag.ApiBaseUrl = _apiBaseUrl;

            var token = User.Claims.FirstOrDefault(c => c.Type == "AuthToken")?.Value;

            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), token);

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Sesion"); // Redirige al login 
            }

            var Datos = await dashboardService.ObtenerDatos(token);

            var NotificacionesHoy = await dashboardService.NotificacionesHoy(token);

            var model = new DashboardViewModel
            {
                Plastico = Datos.Plastico,
                Metal = Datos.Metal,
                TotalAlertas = NotificacionesHoy.Total_alertas
            };

            ViewData["NombreUsuario"] = ConsultaDatosSesion.Nombre;
            ViewData["AreaId"] = ConsultaDatosSesion.AreaId;
            ViewData["Foto"] = ConsultaDatosSesion.Foto == "" ? null : ConsultaDatosSesion.Foto;
            ViewData["Notificacion"] = ConsultaDatosSesion.Notificaciones;

            if (int.Parse(TipoId.Value) != 1)
            {
                return RedirectToAction("Index", "Conjuntos", new { Id = ConsultaDatosSesion.AreaId });
            }
            return View(model);
        }
    }
}
