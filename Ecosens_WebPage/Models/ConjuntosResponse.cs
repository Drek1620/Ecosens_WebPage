using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class ConjuntosResponse
    {
        [JsonPropertyName("areaId")]
        public int AreaId { get; set; }
        [JsonPropertyName("areaNombre")]
        public string Area { get; set; }
        [JsonPropertyName("conjuntos")]
        public List<Conjuntos> Conjuntos { get; set; }
    }

    public class ConjuntoConContenedoresDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("mac_ESP32")]
        public string Mac_ESP32 { get; set; }
        [JsonPropertyName("clavesecreta")]
        public string Clavesecreta { get; set; }
        [JsonPropertyName("area_Id")]
        public int Area_Id { get; set; }
        [JsonPropertyName("area")]
        public string Area { get; set; }
        [JsonPropertyName("contenedores")]
        public List<Contenedores> Contenedores { get; set; }
    }

    public class Conjuntos
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("mac_ESP32")]
        public string Mac_ESP32 { get; set; }
        [JsonPropertyName("clavesecreta")]
        public string Clavesecreta { get; set; }
        [JsonPropertyName("areaId")]
        public int AreaId { get; set; }
        [JsonPropertyName("area")]
        public string Area { get; set; }
        [JsonPropertyName("contenedor_plastico")]
        public string Contenedor_plastico { get; set; }
        [JsonPropertyName("contenedor_metal")]
        public string Contenedor_metal { get; set; }
    }
}
