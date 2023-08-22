using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class UsuarioLoginRespuestaDto
    {
        //Este DTO es para obtener las respuesdas de parte del servidor, como un token de login 
        public UsuarioDatosDto Usuario { get; set; }
        public string Token { get; set; }
    }
}
