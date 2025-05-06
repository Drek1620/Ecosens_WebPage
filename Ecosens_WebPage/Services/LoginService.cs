using Ecosens_WebPage.Models;

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

        public async Task<(bool IsSuccess, string Token, string ErrorMessage)> ObtenerToken(string correo, string password)
        {
            var url = $"{_apiBaseUrl}/api/"
        }
    }
}
