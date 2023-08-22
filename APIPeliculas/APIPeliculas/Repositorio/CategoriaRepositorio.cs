using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Repositorio.IRepositorio;

namespace APIPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDBContext _bd;
        public CategoriaRepositorio(ApplicationDBContext bd)
        {
            _bd = bd;
        }
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Update(categoria);

            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Add(categoria);
            return Guardar();
        }

        public bool ExisteCateogria(string nombre)
        {
            bool valor = _bd.categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

            return valor;
        }

        public bool ExisteCateogria(int id)
        {
            return _bd.categorias.Any(i => i.Id == id);
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _bd.categorias.FirstOrDefault(c => c.Id == categoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
