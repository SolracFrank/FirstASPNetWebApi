using APIPeliculas.Models;

namespace APIPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> GetPeliculas();
        Pelicula GetPelicula(int peliculaId);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        bool Guardar();

        //Metodos para buscar por categoria y por nombre
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);
        ICollection<Pelicula> BuscarPelicula(string nombre);

    }
}
