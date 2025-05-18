using Ecosens_WebPage.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ecosens_WebPage.Services
{
    public class DashboardService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient httpClient;

        public DashboardService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
            this.httpClient = httpClient;
        }

        public async Task<DashboardDatosResponse> ObtenerDatos(string Token)
        {
            var url = $"{_apiBaseUrl}/api/Registros/peso-por-tipo";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token) ;

            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<DashboardDatosResponse>(responseString);

            return responseData;
        }

        public async Task<NotificacionHoyResponse> NotificacionesHoy(string Token)
        {
            var url = $"{_apiBaseUrl}/api/Notificaciones/notificaciones/hoy/total";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<NotificacionHoyResponse>(responseString);

            return responseData;
        }
    }
}
