using Ecosens_WebPage.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ecosens_WebPage.Services
{
    public class AreaService
    {
        private readonly HttpClient httpClient;
        private readonly string _baseUrl;

        public AreaService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            _baseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }

        public async Task<List<AreaResponse>> ObtenerAreas(string Token)
        {
            var url = $"{_baseUrl}/api/Area";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token) ;

            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<List<AreaResponse>>(responseString);

            return responseData;
        }

        public async Task<bool> CrearArea(AreaResponse model,string Token)
        {
            var url = $"{_baseUrl}/api/Area";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var body = new
            {
                nombre = model.Nombre,
                descripcion = model.Descripcion
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url,content);

            return response.IsSuccessStatusCode;
        }
    }
}
