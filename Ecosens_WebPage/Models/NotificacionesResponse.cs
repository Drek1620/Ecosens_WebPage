using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class NotificacionesResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName ("mensaje")]
        public string Mensaje { get; set; }
        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }
        [JsonPropertyName("leido")]
        public bool Leido { get; set; } = false;
    }
}
