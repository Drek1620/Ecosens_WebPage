using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecosens_WebPage.Models;

namespace Ecosens_WebPage.Controllers
{
    public class ContenedoresController : Controller
    {
        private readonly SesionDataService sesionDataService;
        private readonly ContenedoresService contenedoresService;
        private readonly NotificacionService notificacionService;
        private readonly RegistroService registroService;

        public ContenedoresController(SesionDataService sesionDataService, ContenedoresService contenedoresService, NotificacionService notificacionService, RegistroService registroService)
        {
            this.sesionDataService = sesionDataService;
            this.contenedoresService = contenedoresService;
            this.notificacionService = notificacionService;
            this.registroService = registroService;
        }
        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            var userId = User.FindFirst("UserId");
            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), Request.Cookies["AuthToken"].ToString());

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Sesion"); // Redirige al login 
            }

            var ConsultaDatosContenedor = await contenedoresService.ObtenerDatosGenerales(id, Request.Cookies["AuthToken"].ToString());
            var ConsultaRegistrosPlastico = await contenedoresService.ObtenerDatosRegistrosContenedor(ConsultaDatosContenedor.ContenedorPlastico.Id, Request.Cookies["AuthToken"].ToString());
            var ConsultaRegistrosMetal = await contenedoresService.ObtenerDatosRegistrosContenedor(ConsultaDatosContenedor.ContenedorMetal.Id, Request.Cookies["AuthToken"].ToString());
            var ConsultarNotificacionesHoy = await notificacionService.ObtenerNotificacionesPorConjunto(id, Request.Cookies["AuthToken"].ToString());

            var model = new ContenedoresViewModel
            {
                ConjuntoId = id,
                IdContenedorPlastico = ConsultaDatosContenedor.ContenedorPlastico.Id,
                IdContenedorMetal = ConsultaDatosContenedor.ContenedorMetal.Id,
                TotalPlastico = ConsultaRegistrosPlastico.PesoAcumulado,
                TotalMetal = ConsultaRegistrosMetal.PesoAcumulado,
                TotalNotificaciones = ConsultarNotificacionesHoy.Total_alertas,
                EstadoPlastico = ConsultaDatosContenedor.ContenedorPlastico.Estado,
                EstadoMetal = ConsultaDatosContenedor.ContenedorMetal.Estado
            };

            ViewData["NombreUsuario"] = ConsultaDatosSesion.Nombre;
            ViewData["AreaId"] = ConsultaDatosSesion.AreaId;
            ViewData["Foto"] = ConsultaDatosSesion.Foto == "" ? null : ConsultaDatosSesion.Foto;
            ViewData["Notificacion"] = ConsultaDatosSesion.Notificaciones;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VaciarContenedor(int id, int idIndex)
        {
            await registroService.VaciarContenedor(id, Request.Cookies["AuthToken"]?.ToString());
            return RedirectToAction("Index", new { id = idIndex });
        }

        [HttpPost]
        public async Task<IActionResult> VaciarAmbosContenedor(int id1, int id2, int idIndex)
        {
            await registroService.VaciarContenedor(id1, Request.Cookies["AuthToken"]?.ToString());
            await registroService.VaciarContenedor(id2, Request.Cookies["AuthToken"]?.ToString());
            return RedirectToAction("Index", new { id = idIndex });
        }

    }
}
