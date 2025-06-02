namespace EstadisticasRepoEscom.Models
{
    public class EstadisticaSemestreDTO
    {
        public int TagSemestreId { get; set; }
        public string NombreSemestre { get; set; } = string.Empty;
        public int TotalConsultas { get; set; }
        public string? UltimaConsulta { get; set; }
        public double PorcentajeDelTotal { get; set; }
    }
}
