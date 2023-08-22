using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class UsuarioDto
    {
        [Key]
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
