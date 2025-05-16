using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class AreaResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio")]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}
