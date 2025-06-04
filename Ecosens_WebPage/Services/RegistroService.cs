using Ecosens_WebPage.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ecosens_WebPage.Services
{
    public class RegistroService
    {
        private readonly HttpClient httpClient;
        private readonly string _apiBaseUrl;
        public RegistroService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }

        public async Task<bool> VaciarContenedor(int Id, string Token)
        {
            var url = $"{_apiBaseUrl}/api/Registros";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var body = new
            {
                id_contenedor = Id,
                estado = "Vacio",
                peso = 0,
                altura = 0
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {

                return true;
            }

            return false;
        }
    }
}
