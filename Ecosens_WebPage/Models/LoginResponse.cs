using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("id")]
        public int UserId { get; set; }
        [JsonPropertyName("tipo_id")]
        public int TipoId { get; set; }
    }
}
