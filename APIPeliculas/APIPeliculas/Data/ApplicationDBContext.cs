using APIPeliculas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIPeliculas.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options)
        {
               
           
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //Agregar los modelos aquí
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Pelicula> peliculas { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<AppUsuario> appUsuarios { get; set; }
    }
}
