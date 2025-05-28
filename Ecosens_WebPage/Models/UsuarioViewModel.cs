using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class UsuarioViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        // Usuario

        [JsonPropertyName("correo")]

        public string Correo { get; set; }

        [JsonPropertyName("contrasena")]

        public string Contrasena { get; set; }


        [JsonPropertyName("tipoEmpleado")]

        public string Tipoempleado { get; set; }

        // Empleado
        [JsonPropertyName("nombre")]

        public string Nombre { get; set; }

        [JsonPropertyName("apellido")]

        public string Apellido { get; set; }

        [JsonPropertyName("telefono")]

        public string Telefono { get; set; }


        [JsonPropertyName("foto")]
        public string Foto { get; set; }

        [JsonIgnore]
        public IFormFile archivoFoto { get; set; }

        [JsonPropertyName("area")]
        public string Area { get; set; }


        [JsonPropertyName("areaId")]

        public int AreaId { get; set; }

        [JsonPropertyName("tipoId")]

        public int TipoId { get; set; }


    }
}
