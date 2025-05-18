using Ecosens_WebPage.Models;
using System.Net.Http.Headers;
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
    }
}
