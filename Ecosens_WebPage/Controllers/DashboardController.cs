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

        public DashboardController(SesionDataService sesionDataService, DashboardService dashboardService)
        {
            this.sesionDataService = sesionDataService;
            this.dashboardService = dashboardService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var userId = User.FindFirst("UserId");
            var TipoId = User.FindFirst("TipoId");

            if(int.Parse(TipoId.Value) != 1)
            {
                return RedirectToAction("Index","Conjuntos");
            }

            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), Request.Cookies["AuthToken"].ToString());

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Sesion"); // Redirige al login 
            }

            var Datos = await dashboardService.ObtenerDatos(Request.Cookies["AuthToken"].ToString());

            var NotificacionesHoy = await dashboardService.NotificacionesHoy(Request.Cookies["AuthToken"].ToString());

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
            return View(model);
        }
    }
}
