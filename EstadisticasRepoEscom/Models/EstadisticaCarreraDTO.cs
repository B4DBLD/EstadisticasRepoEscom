namespace EstadisticasRepoEscom.Models
{
    public class EstadisticaCarreraDTO
    {
        public int TagCarreraId { get; set; }
        public string NombreCarrera { get; set; } = string.Empty;
        public int TotalConsultas { get; set; }
        public string? UltimaConsulta { get; set; }
        public double PorcentajeDelTotal { get; set; }
    }
}
