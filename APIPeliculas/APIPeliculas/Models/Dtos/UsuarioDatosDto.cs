using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class UsuarioDatosDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
    }
}
