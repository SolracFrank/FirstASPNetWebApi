using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIPeliculas.Models.Dtos
{
    public class PeliculaDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duracion es obligatoria")]
        public int Duracion { get; set; }

        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public int categoriaId { get; set; }
      
    }
}
