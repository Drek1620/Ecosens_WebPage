using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class Contenedores
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("Dimensiones")]
        public decimal Dimensiones { get; set; }
        [JsonPropertyName("peso_Total")]
        public decimal Peso_Total { get; set; }
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "Vacio";
        [JsonPropertyName("tipocont_Id")]
        public int Tipocont_Id { get; set; }
        [JsonPropertyName("conjunto_Id")]
        public int Conjunto_Id { get; set; }
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
