using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class CrearCategoriaDto
    {
        [Required (ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(60,ErrorMessage="El numero maximo de caracteres es 60")]
        public string Nombre { get; set; }
    }
}
