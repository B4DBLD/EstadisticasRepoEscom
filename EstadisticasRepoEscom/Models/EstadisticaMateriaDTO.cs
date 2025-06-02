namespace EstadisticasRepoEscom.Models
{
    public class EstadisticaMateriaDTO
    {
        public int TagMateriaId { get; set; }
        public string NombreMateria { get; set; } = string.Empty;
        public int TotalConsultas { get; set; }
        public string? UltimaConsulta { get; set; }
        public double PorcentajeDelTotal { get; set; }
    }
}
