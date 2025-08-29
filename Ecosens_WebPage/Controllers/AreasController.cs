using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ecosens_WebPage.Models;
using Microsoft.AspNetCore.Http;

namespace Ecosens_WebPage.Controllers
{
    public class AreasController : Controller
    {
        private readonly SesionDataService sesionDataService;
        private readonly AreaService areaService;
        private readonly string token;

        public AreasController(SesionDataService sesionDataService, AreaService areaService)
        {
            this.sesionDataService = sesionDataService;
            this.areaService = areaService;
            token = HttpContext?.User
            .Claims.FirstOrDefault(c => c.Type == "AuthToken")?.Value;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId");
            var TipoId = User.FindFirst("TipoId"); 

            var ConsultarAreas = await areaService.ObtenerAreas(token);

            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), token);

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Sesion"); // Redirige al login 
            }

            ViewData["NombreUsuario"] = ConsultaDatosSesion.Nombre;
            ViewData["AreaId"] = ConsultaDatosSesion.AreaId;
            ViewData["Foto"] = ConsultaDatosSesion.Foto == "" ? null : ConsultaDatosSesion.Foto;
            ViewData["Notificacion"] = ConsultaDatosSesion.Notificaciones;
            if (int.Parse(TipoId.Value) != 1)
            {
                return RedirectToAction("Index", "Conjuntos", new { Id = ConsultaDatosSesion.AreaId });
            }

            return View(ConsultarAreas);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(AreaResponse model)
        {
            if (!ModelState.IsValid)
            {
                // Podrías recargar la vista con errores o redirigir manteniendo los datos
                return PartialView("_FormularioAreaPartial", model);
            }

            await areaService.CrearArea(model, token);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {

            var model = await areaService.ObtenerAreaPorId(id, token);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("_FormularioEditarAreaPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(AreaResponse model)
        {
            if (!ModelState.IsValid)
            {
                // Podrías recargar la vista con errores o redirigir manteniendo los datos
                return PartialView("_FormularioAreaPartial", model);
            }
            await areaService.EditarArea(model, token);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int Id)
        {
            
            await areaService.EliminarAreaPorId(Id, token);

            return RedirectToAction("Index");
        }
    }
}
