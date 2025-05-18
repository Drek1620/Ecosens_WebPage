using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class NotificacionHoyResponse
    {
        [JsonPropertyName("fecha")]
        public DateOnly Fecha { get; set; }
        [JsonPropertyName("total_alertas")]
        public int Total_alertas { get; set; }
    }
}
