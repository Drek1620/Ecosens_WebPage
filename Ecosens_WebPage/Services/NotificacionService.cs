using Ecosens_WebPage.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ecosens_WebPage.Services
{
    public class NotificacionService
    {
        private readonly HttpClient httpClient;
        private readonly string _apiBaseUrl;

        public NotificacionService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }

        public async Task<List<NotificacionesResponse>> ObtenerNotificaciones(int UserId, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Notificaciones/usuario/{UserId}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<List<NotificacionesResponse>>(responseString);

            return responseData;
        }

        public async Task<NotificacionHoyResponse> ObtenerNotificacionesPorConjunto(int Id, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Notificaciones/notificaciones/hoy/conjunto/{Id}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<NotificacionHoyResponse>(responseString);

            return responseData;
        }

        public async Task<bool> MarcarLeido(int Id, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Notificaciones/{Id}/leido";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.PutAsync(url,null);

            return response.IsSuccessStatusCode;
        }
    }
}
