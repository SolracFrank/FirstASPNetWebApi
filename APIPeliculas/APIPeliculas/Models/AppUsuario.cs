using Microsoft.AspNetCore.Identity;

namespace APIPeliculas.Models
{
    public class AppUsuario : IdentityUser
    {
        //Añadir campos personalizados
        public string Nombre { get; set; }
    }
}
