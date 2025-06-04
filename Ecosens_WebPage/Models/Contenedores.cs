using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class Contenedores
    {
        public int Id { get; set; }
        [Required]
        public decimal Dimensiones { get; set; }
        public decimal Peso_Total { get; set; }
        public string Estado { get; set; }
        public int Tipocont_id { get; set; }
        public int Conjunto_id { get; set; }
    }

    public class ContenedoresResponse
    {
        [JsonPropertyName("conjuntoId")]
        public int ConjuntoId { get; set; }
        [JsonPropertyName("contenedorPlastico")]
        public ContenedoresDTO ContenedorPlastico { get; set; }
        [JsonPropertyName("contenedorMetal")]
        public ContenedoresDTO ContenedorMetal { get; set; }

    }

    public class ContenedoresDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("dimensiones")]
        public decimal Dimensiones { get; set; }
        [JsonPropertyName("peso_total")]
        public decimal Peso_Total { get; set; }
        [JsonPropertyName("estado")]
        public string Estado { get; set; }
    }

    public class ContenedoresViewModel
    {
        public int ConjuntoId { get; set; }
        public int IdContenedorPlastico { get; set; }
        public int IdContenedorMetal { get; set; }

        public double TotalPlastico { get; set; }
        public double TotalMetal { get; set; }
        public double Total => TotalPlastico + TotalMetal;
        public int TotalNotificaciones { get; set; }

        public string EstadoPlastico { get; set; }
        public string EstadoMetal { get; set; }
    }
}
