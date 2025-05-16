using Ecosens_WebPage.Models;
using System.Text.Json;
using System.Text;

namespace Ecosens_WebPage.Services
{
    public class LoginService
    {
        private readonly HttpClient httpClient;
        private readonly string _apiBaseUrl;

        public LoginService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            _apiBaseUrl = configuration.GetSection("ApiSettings:BaseUrl").Value;
        }


        public async Task<(bool IsSuccess, string Token,int UserId,int TipoId, string ErrorMessage)> ObtenerToken(string correo, string password)
        {
            var url = $"{_apiBaseUrl}/api/auth/login";

            var body = new
            {
                correo = correo,
                contrasena = password
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<LoginResponse>(responseString);

                if(!string.IsNullOrEmpty(responseData.Token))
                {
                    return (true, responseData.Token,responseData.UserId,responseData.TipoId,null);
                }

                return (false, null, 0, 0, "Usuario o contraseña incorrectos.");
            }

            return (false, null,0, 0, $"Error al autenticar. Codigo de estado: {response.StatusCode}");
        }
    }
}
