using System.ComponentModel.DataAnnotations;

namespace Ecosens_WebPage.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        public bool Recuerdame { get; set; }
    }
}
