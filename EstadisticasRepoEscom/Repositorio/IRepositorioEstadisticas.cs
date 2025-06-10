using EstadisticasRepoEscom.Models;

namespace EstadisticasRepoEscom.Repositorio
{
    public interface IRepositorioEstadisticas
    {
        Task<EstadisticasGeneralesDTO> GetEstadisticasGenerales();
        Task<ResumenEstadisticasDTO> GetResumenEstadisticas();
        Task<List<EstadisticaCarreraDTO>> GetEstadisticasCarreras();
        Task<List<EstadisticaSemestreDTO>> GetEstadisticasSemestres();
        Task<List<EstadisticaMateriaDTO>> GetEstadisticasMaterias(int limit = 10);
        Task<List<EstadisticaAutorDTO>> GetEstadisticasAutores(int limit = 10);
        Task<List<EstadisticaMaterialDTO>> GetEstadisticasMateriales(int limit = 10);
        Task<List<EstadisticaMaterialesPorAutorDTO>> GetEstadisticasMaterialesPorAutor(int limit = 10);
    }
}
