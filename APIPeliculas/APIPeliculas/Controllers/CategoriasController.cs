// Importar los espacios de nombres necesarios
using APIPeliculas.Models;
using APIPeliculas.Models.Dtos;
using APIPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace APIPeliculas.Controllers
{
    
    // Definir la ruta base y que es un controlador de API
    [Route("api/categorias")]
  //[Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo; // Repositorio para categorías
        private readonly IMapper _mapper; // Instancia de AutoMapper para mapeo de objetos

        // Constructor del controlador, se inyectan las dependencias
        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo; // Asignar el repositorio de categorías
            _mapper = mapper; // Asignar el mapeador
        }
        //        
        // ENDPOINT de Acción para obtener todas las categorías, responde a solicitudes HTTP GET (VERBO)
        [AllowAnonymous]
        [HttpGet]
        //[ResponseCache(Duration = 20)]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            // Obtener la lista de categorías desde el repositorio
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>(); // Crear una lista para los DTOs

            // Iterar a través de las categorías y mapearlas a DTOs
            foreach (var categoria in listaCategorias)
            {
                var categoriaDto = _mapper.Map<CategoriaDto>(categoria);
                listaCategoriasDto.Add(categoriaDto);
            }

            // Devolver una respuesta OK con la lista de categorías en formato DTO
            return Ok(listaCategoriasDto);
        }
        //End Point para categoria por ID
        [AllowAnonymous]
        [HttpGet ("{categoriaId:int}",Name = "GetCategoria")]
        [ResponseCache(CacheProfileName = "PorDefecto20")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);
            if (itemCategoria==null)
            {
                return NotFound();
            }

            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);

            return Ok(itemCategoriaDto); 
        }
        //End Point para crear categoría
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type= typeof (CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid || crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRepo.ExisteCateogria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
            if(!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, enviando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new {categoriaId = categoria.Id},categoria);
        }
        //End Point para Actualizar Categoria Parcialmente
        [Authorize(Roles = "admin")]
        [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid || categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
          

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, enviando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //End Point para borrar categoria
        [Authorize(Roles = "admin")]
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if (!_ctRepo.ExisteCateogria(categoriaId))
            {
                return NotFound();
            }

            var categoria = _ctRepo.GetCategoria(categoriaId);
            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal, borrando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
