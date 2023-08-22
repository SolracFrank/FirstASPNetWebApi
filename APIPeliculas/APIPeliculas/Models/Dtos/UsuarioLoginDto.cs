using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
