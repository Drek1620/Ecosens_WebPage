using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class DatosRegistrosContenedorResponse
    {
        [JsonPropertyName("contenedorId")]
        public int ContenedorId { get; set; }
        [JsonPropertyName("desde")]
        public DateTime Desde { get; set; }
        [JsonPropertyName("pesoAcumulado")]
        public double PesoAcumulado { get; set; }
    }
}
