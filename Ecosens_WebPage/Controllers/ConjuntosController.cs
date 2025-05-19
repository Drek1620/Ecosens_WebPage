using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecosens_WebPage.Models;

namespace Ecosens_WebPage.Controllers
{
    public class ConjuntosController : Controller
    {
        private readonly ConjuntoService conjuntoService;
        private readonly SesionDataService sesionDataService;

        public ConjuntosController(ConjuntoService conjuntoService, SesionDataService sesionDataService)
        {
            this.conjuntoService = conjuntoService;
            this.sesionDataService = sesionDataService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            var userId = User.FindFirst("UserId");
            var model = await conjuntoService.ObtenerConjuntosPorArea(Id, Request.Cookies["AuthToken"].ToString());

            if(model == null)
            {
                return RedirectToAction("Index", "Area");
            }

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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ConjuntoConContenedoresDto model)
        {
            if (!ModelState.IsValid)
            {
                // Podrías recargar la vista con errores o redirigir manteniendo los datos
                return PartialView("_FormularioAreaPartial", model);
            }

            await conjuntoService.CrearConjunto(model, Request.Cookies["AuthToken"].ToString());

            return RedirectToAction("Index", new {id=model.Area_id});
        }
    }
}
