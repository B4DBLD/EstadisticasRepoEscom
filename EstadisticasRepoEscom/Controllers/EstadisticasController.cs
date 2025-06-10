using EstadisticasRepoEscom.Conexion;
using EstadisticasRepoEscom.Models;
using EstadisticasRepoEscom.Repositorio;
using MicroserviciosRepoEscom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;

namespace EstadisticasRepoEscom.Controllers
{
    [ApiController]
    [Route("repositorio/[controller]")]
    public class EstadisticasController : ControllerBase
    {
        private readonly IRepositorioEstadisticas _estadisticasRepository;
        private readonly ILogger<EstadisticasController> _logger;
        private readonly DBConfig _dbConfig;

        public EstadisticasController(
            IRepositorioEstadisticas estadisticasRepository,
            ILogger<EstadisticasController> logger,
            DBConfig dbConfig)
        {
            _estadisticasRepository = estadisticasRepository;
            _logger = logger;
            _dbConfig = dbConfig;
        }

        /// <summary>
        /// Obtiene todas las estadísticas generales del repositorio
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<EstadisticasGeneralesDTO>>> GetEstadisticasGenerales()
        {
            try
            {
                var estadisticas = await _estadisticasRepository.GetEstadisticasGenerales();
                return Ok(ApiResponse<EstadisticasGeneralesDTO>.Success(
                    estadisticas,
                    "Estadísticas generales obtenidas exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas generales");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene un resumen ejecutivo de las estadísticas
        /// </summary>
        [HttpGet("resumen")]
        public async Task<ActionResult<ApiResponse<ResumenEstadisticasDTO>>> GetResumenEstadisticas()
        {
            try
            {
                var resumen = await _estadisticasRepository.GetResumenEstadisticas();
                return Ok(ApiResponse<ResumenEstadisticasDTO>.Success(
                    resumen,
                    "Resumen de estadísticas obtenido exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resumen de estadísticas");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene estadísticas de carreras más consultadas
        /// </summary>
        [HttpGet("carreras")]
        public async Task<ActionResult<ApiResponse<List<EstadisticaCarreraDTO>>>> GetEstadisticasCarreras()
        {
            try
            {
                var carreras = await _estadisticasRepository.GetEstadisticasCarreras();
                return Ok(ApiResponse<List<EstadisticaCarreraDTO>>.Success(
                    carreras,
                    $"Total de consultas por carrera"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de carreras");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene estadísticas de semestres más consultados
        /// </summary>
        [HttpGet("semestres")]
        public async Task<ActionResult<ApiResponse<List<EstadisticaSemestreDTO>>>> GetEstadisticasSemestres()
        {
            try
            {

                var semestres = await _estadisticasRepository.GetEstadisticasSemestres();
                return Ok(ApiResponse<List<EstadisticaSemestreDTO>>.Success(
                    semestres,
                    $"Top {semestres.Count} semestres más consultados obtenidos exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de semestres");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene estadísticas de materias más consultadas
        /// </summary>
        [HttpGet("materias")]
        public async Task<ActionResult<ApiResponse<List<EstadisticaMateriaDTO>>>> GetEstadisticasMaterias(
            [FromQuery] int limit = 10)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                {
                    return BadRequest(ApiResponse.Failure("El límite debe estar entre 1 y 100"));
                }

                var materias = await _estadisticasRepository.GetEstadisticasMaterias(limit);
                return Ok(ApiResponse<List<EstadisticaMateriaDTO>>.Success(
                    materias,
                    $"Top {materias.Count} materias más consultadas obtenidas exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de materias");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene estadísticas de autores más consultados
        /// </summary>
        [HttpGet("autores")]
        public async Task<ActionResult<ApiResponse<List<EstadisticaAutorDTO>>>> GetEstadisticasAutores([FromQuery] int limit = 10)
        {
            try
            {

                var autores = await _estadisticasRepository.GetEstadisticasAutores();
                return Ok(ApiResponse<List<EstadisticaAutorDTO>>.Success(
                    autores,
                    $"Top {autores.Count} autores más consultados obtenidos exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de autores");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene estadísticas de materiales más consultados
        /// </summary>
        [HttpGet("materiales")]
        public async Task<ActionResult<ApiResponse<List<EstadisticaMaterialDTO>>>> GetEstadisticasMateriales(
            [FromQuery] int limit = 10)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                {
                    return BadRequest(ApiResponse.Failure("El límite debe estar entre 1 y 100"));
                }

                var materiales = await _estadisticasRepository.GetEstadisticasMateriales(limit);
                return Ok(ApiResponse<List<EstadisticaMaterialDTO>>.Success(
                    materiales,
                    $"Top {materiales.Count} materiales más consultados obtenidos exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de materiales");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtiene estadísticas de la cantidad de materiales subidos por autor
        /// </summary>
        [HttpGet("materiales-por-creador")]
        public async Task<ActionResult<ApiResponse<List<EstadisticaMaterialesPorAutorDTO>>>> GetEstadisticasMaterialesPorAutor(
            [FromQuery] int limit = 10)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                {
                    return BadRequest(ApiResponse.Failure("El límite debe estar entre 1 y 100"));
                }

                var materialesPorAutor = await _estadisticasRepository.GetEstadisticasMaterialesPorAutor(limit);
                return Ok(ApiResponse<List<EstadisticaMaterialesPorAutorDTO>>.Success(
                    materialesPorAutor,
                    $"Top {materialesPorAutor.Count} autores con más materiales subidos obtenidos exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de materiales por autor");
                return StatusCode(500, ApiResponse.Failure("Error interno del servidor"));
            }
        }
    }
}
