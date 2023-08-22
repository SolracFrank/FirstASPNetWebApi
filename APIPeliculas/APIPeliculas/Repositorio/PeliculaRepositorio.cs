using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace APIPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDBContext _bd;
        public PeliculaRepositorio(ApplicationDBContext bd)
        {
            _bd = bd;
        }
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Update(pelicula);

            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.peliculas.Remove(pelicula);
            return Guardar();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            return query.ToList();

        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

            return valor;
        }

        public bool ExistePelicula(int id)
        {
            return _bd.peliculas.Any(i => i.Id == id);
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _bd.peliculas.FirstOrDefault(c => c.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {
            return _bd.peliculas.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == catId).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
