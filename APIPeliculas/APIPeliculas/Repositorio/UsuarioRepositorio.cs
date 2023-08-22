using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;


namespace APIPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDBContext _bd;
        private string claveSecreta;
        public UsuarioRepositorio(ApplicationDBContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }
        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.usuarios.OrderBy(u => u.NombreUsuario).ToList();
        }

        public bool Guardar()
        {
            throw new NotImplementedException();
        }

        public bool isUniqueUser(string nombre)
        {
            var usuarioBd = _bd.usuarios.FirstOrDefault(u => u.NombreUsuario == nombre);
             if (usuarioBd==null)
            {
                return true;
            }
            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptada = Obtenermd5(usuarioLoginDto.Password);
            var usuario = _bd.usuarios.FirstOrDefault(
                    u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower() &&
                    u.Password == passwordEncriptada
                );
            //validamos si el usuario no existe con la combinación de pass y user
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Si existe
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = manejadorToken.CreateToken(tokenDescription);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario =  usuario
            };
            return usuarioLoginRespuestaDto;

        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = Obtenermd5(usuarioRegistroDto.Password);
            usuarioRegistroDto.Password = passwordEncriptado;
            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = usuarioRegistroDto.Password,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role
            };
            _bd.Add(usuario);
            await _bd.SaveChangesAsync();
           
            return usuario;
        }

        //Encriptas método
        public static string Obtenermd5(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = md5.ComputeHash(data);

            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }

            return resp;
        }
    }
}
