using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class SesionDataResponse
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("areaId")]
        public int? AreaId { get; set; }
        [JsonPropertyName("foto")]
        public string Foto { get; set; }
        [JsonPropertyName("notificaciones")]
        public int Notificaciones { get; set; }
    }
}
