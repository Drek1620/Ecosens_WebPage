using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Ecosens_WebPage.Models;

namespace Ecosens_WebPage.Controllers
{
    public class PerfilController : Controller
    {
        private readonly SesionDataService sesionDataService;
        private readonly EmpleadoServices empleadoServices;
        private readonly string _token;

        public PerfilController(SesionDataService sesionDataService,EmpleadoServices empleadoServices)
        {
            this.sesionDataService = sesionDataService;
            this.empleadoServices = empleadoServices;
            _token = sesionDataService.GetToken();
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("UserId");
            var tipoId = User.FindFirst("TipoId");
            var ConsultarDatosPerfil = await empleadoServices.ObtenerDatosPerfilActual(int.Parse(userId.Value), _token);

            var ConsultaDatosSesion = await sesionDataService.ObtenerDatosSesion(int.Parse(userId.Value), _token);

            if (!ConsultaDatosSesion.IsSuccess)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Dashboard", "Index"); // Redirige al login 
            }

            if (ConsultarDatosPerfil == null)
            {
                return View();
            }

            ViewData["NombreUsuario"] = ConsultaDatosSesion.Nombre;
            ViewData["AreaId"] = ConsultaDatosSesion.AreaId;
            ViewData["Foto"] = ConsultaDatosSesion.Foto == "" ? null : ConsultaDatosSesion.Foto;
            ViewData["Notificacion"] = ConsultaDatosSesion.Notificaciones;

            return View(ConsultarDatosPerfil); 
        }

        public async Task<IActionResult> Editar(UsuarioViewModel model)
        {
            return View();
        }
    }
}
