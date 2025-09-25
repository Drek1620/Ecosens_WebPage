using Ecosens_WebPage.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ecosens_WebPage.Services
{
    public class ConjuntoService
    {
        private readonly HttpClient httpClient;
        private readonly string _apiBaseUrl;

        public ConjuntoService(HttpClient httpClient,IConfiguration configuration)
        {
            this.httpClient = httpClient;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }

        public async Task<ConjuntosResponse> ObtenerConjuntosPorArea(int Id, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Conjuntos/area/{Id}/conjuntos-estados";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ConjuntosResponse>(responseString);

            return responseData;
        }

        public async Task<string> CrearConjunto(ConjuntoConContenedoresDto model, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Conjuntos/conjunto-con-contenedores";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var body = new
            {
                mac_ESP32 = model.Mac_ESP32,
                clavesecreta = model.Clavesecreta,
                area_Id = model.Area_Id,
                contenedores = model.Contenedores
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return $"Éxito: {responseContent}";
            }
            else
            {
                return $"Error: {responseContent}";
            }
        }

        public async Task<ConjuntoConContenedoresDto> ObtenerConjuntoPorId(int id, string token)
        {
            var url = $"{_apiBaseUrl}/api/Conjuntos/conjunto-con-contenedores/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonSerializer.Deserialize<ConjuntoConContenedoresDto>(responseContent);
                return responseData;
            }
            else
            {
                return null;
            }
        }

    }
}
