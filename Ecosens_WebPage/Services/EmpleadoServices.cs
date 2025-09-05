using Ecosens_WebPage.Models;

namespace Ecosens_WebPage.Services
{
    public class EmpleadoServices
    {
        private readonly HttpClient httpClient;
        private readonly string _apiBaseUrl;
        public EmpleadoServices(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }

        // Implementar métodos para interactuar con la API de empleados
        public async Task<UsuarioViewModel> ObtenerDatosPerfilActual(int empleadoId, string token)
        {
            var url = $"{_apiBaseUrl}/api/Empleados/{empleadoId}";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseString = await response.Content.ReadAsStringAsync();
            var empleado = System.Text.Json.JsonSerializer.Deserialize<UsuarioViewModel>(responseString);
            return empleado;

        }
    }
}
