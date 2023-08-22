using APIPeliculas.Data;
using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UsuarioRepositorio(ApplicationDBContext bd, IConfiguration config, 
            RoleManager<IdentityRole> roleManager, UserManager<AppUsuario> userManager, IMapper mapper)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.appUsuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.appUsuarios.OrderBy(u => u.UserName).ToList();
        }

        public bool Guardar()
        {
            throw new NotImplementedException();
        }

        public bool isUniqueUser(string usuario)
        {
            var usuarioBd = _bd.appUsuarios.FirstOrDefault(u => u.UserName == usuario);
             if (usuarioBd==null)
            {
                return true;
            }
            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
         //   var passwordEncriptada = Obtenermd5(usuarioLoginDto.Password);
            var usuario = _bd.appUsuarios.FirstOrDefault(
                    u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());
            bool isValide = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);

            //validamos si el usuario no existe con la combinación de pass y user
            if (usuario == null || !isValide)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Si existe
            var roles = await _userManager.GetRolesAsync(usuario);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescription);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)
            };
            return usuarioLoginRespuestaDto;

        }

        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
                
            };
            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);
            if(result.Succeeded)
            {
                //Solo la primera vez para crear los roles
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }
                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioRetornado = _bd.appUsuarios.FirstOrDefault(u =>
                u.UserName == usuarioRegistroDto.NombreUsuario);
                //opcion 1
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre
                //};
                //opcion 2
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }
            return new UsuarioDatosDto();
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
