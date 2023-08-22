using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using AutoMapper;

namespace APIPeliculas.PeliculasMapper
{
    public class PeliculaMapper :Profile
    {
        public PeliculaMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();

            CreateMap<Pelicula, PeliculaDto>().ReverseMap();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioRegistroDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginRespuestaDto>().ReverseMap();

            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
        }
    }
}
