namespace EstadisticasRepoEscom.Models
{
    public class EstadisticasGeneralesDTO
    {
        public ResumenEstadisticasDTO Resumen { get; set; } = new();
        public List<EstadisticaCarreraDTO> TopCarreras { get; set; } = new();
        public List<EstadisticaSemestreDTO> TopSemestres { get; set; } = new();
        public List<EstadisticaMateriaDTO> TopMaterias { get; set; } = new();
        public List<EstadisticaAutorDTO> TopAutores { get; set; } = new();
        public List<EstadisticaMaterialDTO> TopMateriales { get; set; } = new();
    }
}
