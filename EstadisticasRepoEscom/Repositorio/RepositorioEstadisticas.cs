using EstadisticasRepoEscom.Conexion;
using EstadisticasRepoEscom.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace EstadisticasRepoEscom.Repositorio
{
    public class RepositorioEstadisticas : IRepositorioEstadisticas
    {
        private readonly DBConfig _dbConfig;
        private readonly ILogger<RepositorioEstadisticas> _logger;

        public RepositorioEstadisticas(DBConfig dbConfig, ILogger<RepositorioEstadisticas> logger)
        {
            _dbConfig = dbConfig;
            _logger = logger;
        }

        public async Task<EstadisticasGeneralesDTO> GetEstadisticasGenerales()
        {
            return new EstadisticasGeneralesDTO
            {
                Resumen = await GetResumenEstadisticas(),
                TopCarreras = await GetEstadisticasCarreras(),
                TopSemestres = await GetEstadisticasSemestres(),
                TopMaterias = await GetEstadisticasMaterias(10),
                TopAutores = await GetEstadisticasAutores(),
                TopMateriales = await GetEstadisticasMateriales(20)
            };
        }

        public async Task<ResumenEstadisticasDTO> GetResumenEstadisticas()
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        (SELECT COUNT(*) FROM UserSearch) as totalConsultas,
                        (SELECT COUNT(*) FROM VistaEstadisticasCarreras WHERE totalConsultas > 0) as totalCarreras,
                        (SELECT COUNT(*) FROM VistaEstadisticasSemestres WHERE totalConsultas > 0) as totalSemestres,
                        (SELECT COUNT(*) FROM VistaEstadisticasMaterias WHERE totalConsultas > 0) as totalMaterias,
                        (SELECT COUNT(*) FROM VistaEstadisticasAutores WHERE totalConsultas > 0) as totalAutores,
                        (SELECT COUNT(*) FROM VistaEstadisticasMateriales WHERE totalConsultas > 0) as totalMateriales,
                        (SELECT nombreCarrera FROM VistaEstadisticasCarreras LIMIT 1) as carreraMasConsultada,
                        (SELECT nombreCompleto FROM VistaEstadisticasAutores LIMIT 1) as autorMasConsultado,
                        (SELECT nombreMaterial FROM VistaEstadisticasMateriales LIMIT 1) as materialMasConsultado";

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new ResumenEstadisticasDTO
                    {
                        TotalConsultasHistorico = reader.GetInt32("totalConsultas"),
                        TotalCarrerasConsultadas = reader.GetInt32("totalCarreras"),
                        TotalSemestresConsultados = reader.GetInt32("totalSemestres"),
                        TotalMateriasConsultadas = reader.GetInt32("totalMaterias"),
                        TotalAutoresConsultados = reader.GetInt32("totalAutores"),
                        TotalMaterialesConsultados = reader.GetInt32("totalMateriales"),
                        CarreraMasConsultada = reader.IsDBNull("carreraMasConsultada") ? null : reader.GetString("carreraMasConsultada"),
                        AutorMasConsultado = reader.IsDBNull("autorMasConsultado") ? null : reader.GetString("autorMasConsultado"),
                        MaterialMasConsultado = reader.IsDBNull("materialMasConsultado") ? null : reader.GetString("materialMasConsultado")
                    };
                }

                return new ResumenEstadisticasDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener resumen de estadísticas");
                return new ResumenEstadisticasDTO();
            }
        }
        public async Task<List<EstadisticaCarreraDTO>> GetEstadisticasCarreras()
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT tagId, nombreCarrera, totalConsultas, ultimaConsulta, porcentajeDelTotal
                    FROM VistaEstadisticasCarreras";

                var carreras = new List<EstadisticaCarreraDTO>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    carreras.Add(new EstadisticaCarreraDTO
                    {
                        TagCarreraId = reader.GetInt32("tagId"),
                        NombreCarrera = reader.GetString("nombreCarrera"),
                        TotalConsultas = reader.GetInt32("totalConsultas"),
                        UltimaConsulta = reader.IsDBNull("ultimaConsulta") ? null : reader.GetString("ultimaConsulta"),
                        PorcentajeDelTotal = reader.GetDouble("porcentajeDelTotal")
                    });
                }

                return carreras;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener estadísticas de carreras");
                return new List<EstadisticaCarreraDTO>();
            }
        }

        public async Task<List<EstadisticaSemestreDTO>> GetEstadisticasSemestres()
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT tagId, nombreSemestre, totalConsultas, ultimaConsulta, porcentajeDelTotal
                    FROM VistaEstadisticasSemestres";

                var semestres = new List<EstadisticaSemestreDTO>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    semestres.Add(new EstadisticaSemestreDTO
                    {
                        TagSemestreId = reader.GetInt32("tagId"),
                        NombreSemestre = reader.GetString("nombreSemestre"),
                        TotalConsultas = reader.GetInt32("totalConsultas"),
                        UltimaConsulta = reader.IsDBNull("ultimaConsulta") ? null : reader.GetString("ultimaConsulta"),
                        PorcentajeDelTotal = reader.GetDouble("porcentajeDelTotal")
                    });
                }

                return semestres;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener estadísticas de semestres");
                return new List<EstadisticaSemestreDTO>();
            }
        }

        public async Task<List<EstadisticaMateriaDTO>> GetEstadisticasMaterias(int limit = 10)
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT tagId, nombreMateria, totalConsultas, ultimaConsulta, porcentajeDelTotal
                    FROM VistaEstadisticasMaterias
                    LIMIT @limit";

                command.Parameters.AddWithValue("@limit", limit);

                var materias = new List<EstadisticaMateriaDTO>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    materias.Add(new EstadisticaMateriaDTO
                    {
                        TagMateriaId = reader.GetInt32("tagId"),
                        NombreMateria = reader.GetString("nombreMateria"),
                        TotalConsultas = reader.GetInt32("totalConsultas"),
                        UltimaConsulta = reader.IsDBNull("ultimaConsulta") ? null : reader.GetString("ultimaConsulta"),
                        PorcentajeDelTotal = reader.GetDouble("porcentajeDelTotal")
                    });
                }

                return materias;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener estadísticas de materias");
                return new List<EstadisticaMateriaDTO>();
            }
        }

        public async Task<List<EstadisticaAutorDTO>> GetEstadisticasAutores(int limit = 10)
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT autorId, nombreCompleto, email, totalConsultas, ultimaConsulta, porcentajeDelTotal
                    FROM VistaEstadisticasAutores
                    LIMIT @limit";

                command.Parameters.AddWithValue("@limit", limit);

                var autores = new List<EstadisticaAutorDTO>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    autores.Add(new EstadisticaAutorDTO
                    {
                        AutorId = reader.GetInt32("autorId"),
                        NombreCompleto = reader.GetString("nombreCompleto"),
                        Email = reader.GetString("email"),
                        TotalConsultas = reader.GetInt32("totalConsultas"),
                        UltimaConsulta = reader.IsDBNull("ultimaConsulta") ? null : reader.GetString("ultimaConsulta"),
                        PorcentajeDelTotal = reader.GetDouble("porcentajeDelTotal")
                    });
                }

                return autores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener estadísticas de autores");
                return new List<EstadisticaAutorDTO>();
            }
        }

        public async Task<List<EstadisticaMaterialDTO>> GetEstadisticasMateriales(int limit = 10)
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);
            await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT materialId, nombreMaterial, tipoArchivo, disponible, totalConsultas, ultimaConsulta, porcentajeDelTotal
                    FROM VistaEstadisticasMateriales
                    LIMIT @limit";

                command.Parameters.AddWithValue("@limit", limit);

                var materiales = new List<EstadisticaMaterialDTO>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    materiales.Add(new EstadisticaMaterialDTO
                    {
                        MaterialId = reader.GetInt32("materialId"),
                        NombreMaterial = reader.GetString("nombreMaterial"),
                        TipoArchivo = reader.GetString("tipoArchivo"),
                        Disponible = reader.GetInt32("disponible") == 1,
                        TotalConsultas = reader.GetInt32("totalConsultas"),
                        UltimaConsulta = reader.IsDBNull("ultimaConsulta") ? null : reader.GetString("ultimaConsulta"),
                        PorcentajeDelTotal = reader.GetDouble("porcentajeDelTotal")
                    });
                }

                return materiales;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener estadísticas de materiales");
                return new List<EstadisticaMaterialDTO>();
            }
        }

    }
}
