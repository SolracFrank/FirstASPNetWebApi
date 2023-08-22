using APIPeliculas.Models;

namespace APIPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int categoriaId);
        bool ExisteCateogria(string nombre);
        bool ExisteCateogria(int id);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();



    }
}
