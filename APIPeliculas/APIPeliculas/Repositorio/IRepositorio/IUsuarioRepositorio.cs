using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;

namespace APIPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string usuarioId);
        bool isUniqueUser(string nombre);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        //Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);

        bool Guardar();



    }
}
