using Ecosens_WebPage.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ecosens_WebPage.Services
{
    public class ContenedoresService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient httpClient;
        public ContenedoresService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
            this.httpClient = httpClient;
        }

        public async Task<ContenedoresResponse> ObtenerDatosGenerales(int id, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Contenedores/conjunto/{id}/contenedores";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ContenedoresResponse>(responseString);

            return responseData;
        }

        public async Task<DatosRegistrosContenedorResponse> ObtenerDatosRegistrosContenedor(int id, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Registros/contenedor/{id}/peso-desde-ultimo-vacio-parcial";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<DatosRegistrosContenedorResponse>(responseString);

            return responseData;
        }
    }
}
