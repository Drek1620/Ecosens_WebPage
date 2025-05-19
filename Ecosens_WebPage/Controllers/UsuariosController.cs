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

namespace Ecosens_WebPage.Controllers
{
    public class UsuariosController : Controller
    {

        Uri baseAddress = new Uri("https://ecosensapi20250513230303.azurewebsites.net/api/");
        private readonly HttpClient _client;
        private readonly SesionDataService sesionDataService;

        public UsuariosController(SesionDataService sesionDataService)
        {

            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.sesionDataService = sesionDataService;
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
        public IActionResult Create(UsuarioViewModel model)
        {
            try
            {
                string datos = JsonSerializer.Serialize(model);
                StringContent contenido = new StringContent(datos, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "Empleados", contenido).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["datoChido"] = "se agrego el empleado correctamente.";
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "no se pudo crear el empleado";
                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }




        [HttpGet]
        public IActionResult Edit(int id)
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
        public IActionResult Edit(UsuarioViewModel model)
        {
            try
            {
                string datos = JsonSerializer.Serialize(model);
                StringContent contenido = new StringContent(datos, Encoding.UTF8, "application/json");
                var url = $"Empleados/{model.Id}";
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + url, contenido).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["datoChido"] = "se agrego el empleado correctamente.";
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "no se pudo crear el empleado";
                return RedirectToAction("Index");

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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "Empleados/" +  id).Result;

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
