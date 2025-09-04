using Ecosens_WebPage.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Ecosens_WebPage.Services
{
    public class SesionDataService
    {
        private readonly HttpClient httpClient;
        private readonly string _apiBaseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SesionDataService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool IsSuccess, string Nombre, int? AreaId, string Foto, int Notificaciones, string ErrorMessage)> ObtenerDatosSesion(int UserId, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Empleados/Sesion/{UserId}";

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<SesionDataResponse>(responseString);

                if (responseData is not null)
                {
                    return (true, responseData.Nombre, responseData.AreaId, responseData.Foto, responseData.Notificaciones, null);
                }

                return (false, null, 0, null, 0, "El ID del usuario es incorrecto o no se encontró.");

            }

            return (false, null, 0, null, 0, $"Error al autenticar. Codigo de estado: {response.StatusCode}");
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "AuthToken")?.Value;
        }
    }
}
