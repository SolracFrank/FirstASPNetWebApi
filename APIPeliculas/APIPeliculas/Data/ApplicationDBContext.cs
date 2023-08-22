using APIPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPeliculas.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options)
        {
               
           
        }
        //Agregar los modelos aquí
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Pelicula> peliculas { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
    }
}
