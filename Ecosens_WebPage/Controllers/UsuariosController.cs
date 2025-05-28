using Ecosens_WebPage.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ecosens_WebPage.Services;
using Ecosens_WebPage.Helpers;

namespace Ecosens_WebPage.Controllers
{
    public class UsuariosController : Controller
    {

        Uri baseAddress = new Uri("https://ecosensapi20250513230303.azurewebsites.net/api/");
        private readonly HttpClient _client;
        private readonly SesionDataService sesionDataService;
        private readonly IConfiguration _config;

        public UsuariosController(SesionDataService sesionDataService, IConfiguration config)
        {

            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.sesionDataService = sesionDataService;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = null)
        {
            // 1. Trae la lista de la API
            var response = _client.GetAsync("Empleados/usuario").Result;
            var list = new List<UsuarioViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                list = JsonSerializer.Deserialize<List<UsuarioViewModel>>(
                    data,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }

            // 2. Filtra si searchTerm no es nulo o vacío
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lower = searchTerm.Trim().ToLower();
                list = list.Where(u =>
                    (u.Nombre ?? "").ToLower().Contains(lower) ||
                    (u.Apellido ?? "").ToLower().Contains(lower) ||
                    (u.Correo ?? "").ToLower().Contains(lower) ||
                    (u.Telefono ?? "").ToLower().Contains(lower) ||
                    (u.Tipoempleado ?? "").ToLower().Contains(lower) ||
                    (u.Area ?? "").ToLower().Contains(lower)
                ).ToList();
            }

            // 3. Pasa el valor al ViewData para rellenar el input
            ViewData["SearchTerm"] = searchTerm;
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

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            try
            {
                var storageConfig = new AzureStorageConfig
                {
                    Cuenta = _config["AzureStorage:Cuenta"],
                    Llave = _config["AzureStorage:Llave"],
                    Contenedor = _config["AzureStorage:Contenedor"]
                };

                if (model.archivoFoto != null && model.archivoFoto.Length > 0)
                {
                    var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(model.archivoFoto.FileName)}";
                    model.Foto = await StorageHelpers.SubirArchivo(
                        model.archivoFoto.OpenReadStream(),
                        nombreArchivo,
                        storageConfig
                    );
                }

                string datos = JsonSerializer.Serialize(model);
                StringContent contenido = new(datos, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "Empleados", contenido);

                if (response.IsSuccessStatusCode)
                {
                    TempData["datoChido"] = "Empleado creado exitosamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error creando empleado: {ex.Message}";
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"Empleados/{id}");

            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var modelo = JsonSerializer.Deserialize<UsuarioViewModel>(contenido);
                return View(modelo);
            }

            TempData["error"] = "Empleado no encontrado";
            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioViewModel model)
        {
            try
            {
                var storageConfig = new AzureStorageConfig
                {
                    Cuenta = _config["AzureStorage:Cuenta"],
                    Llave = _config["AzureStorage:Llave"],
                    Contenedor = _config["AzureStorage:Contenedor"]
                };

                if (model.archivoFoto != null && model.archivoFoto.Length > 0)
                {
                    var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(model.archivoFoto.FileName)}";
                    model.Foto = await StorageHelpers.SubirArchivo(
                        model.archivoFoto.OpenReadStream(),
                        nombreArchivo,
                        storageConfig
                    );
                }

                string datos = JsonSerializer.Serialize(model);
                StringContent contenido = new(datos, Encoding.UTF8, "application/json");
                var url = $"Empleados/{model.Id}";

                HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + url, contenido);

                if (response.IsSuccessStatusCode)
                {
                    TempData["datoChido"] = "Empleado actualizado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error actualizando empleado: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult Delete(int id)
        {
            try
            {
                UsuarioViewModel datos = new UsuarioViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Empleados/" + id).Result;

                if (response.IsSuccessStatusCode)
                {

                    string data = response.Content.ReadAsStringAsync().Result;
                    datos = JsonSerializer.Deserialize<UsuarioViewModel>(data);
                }

                return View(datos);

            }
            catch (Exception ex)
            {
                TempData["error"] = "no se pudo crear el empleado";
                return RedirectToAction("Index");

            }

        }


        [HttpPost]

        public IActionResult Deletecontinuar(int id)
        {

            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "Empleados/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["datoChido"] = "se elimino el empleado correctamente.";

                }
                return RedirectToAction("Index");

            }

            catch (Exception ex)
            {
                TempData["error"] = "no se pudo crear el empleado";
                return RedirectToAction("Index");
            }



        }
















    }
}
