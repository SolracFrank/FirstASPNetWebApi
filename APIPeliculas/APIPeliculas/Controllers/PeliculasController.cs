using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace APIPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo; 
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper; 
        }
        // ENDPOIN para obtener todas las peliculas
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();
            var listaPeliculasDtos = new List<PeliculaDto>(); 

            foreach (var pelicula in listaPeliculas)
            {
                var peliculaDto = _mapper.Map<PeliculaDto>(pelicula);
                listaPeliculasDtos.Add(peliculaDto);
            }

            return Ok(listaPeliculasDtos);
        }
        //End Point para pelicula por ID
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);
            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);

            return Ok(itemPeliculaDto);
        }
        //End Point para crear pelicula
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid || peliculaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_pelRepo.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La Pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal, enviando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        //End Point para Actualizar Pelicula Parcialmente
        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid || peliculaDto == null || peliculaId != peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }


            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal, enviando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //End Point para borrar pelicula
        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _pelRepo.GetPelicula(peliculaId);
            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal, borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //End Point para pelicula por categoria
        [AllowAnonymous]
        [HttpGet("GetPeliculaEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPeliculaEnCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);
            if (listaPeliculas == null)
            {
                return NotFound();
            }
            var itemPelicula = new List<PeliculaDto>();
            foreach (var lista in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(itemPelicula);
        }

        //End Point para buscar pelicula por nombre
        [AllowAnonymous]
        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre.Trim());
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }
    }
}
