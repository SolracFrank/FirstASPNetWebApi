using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;

namespace APIPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int usuarioId);
        bool isUniqueUser(string nombre);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);

        bool Guardar();



    }
}
