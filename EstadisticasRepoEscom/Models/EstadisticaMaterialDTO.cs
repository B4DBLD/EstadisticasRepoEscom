namespace EstadisticasRepoEscom.Models
{
    public class EstadisticaMaterialDTO
    {
        public int MaterialId { get; set; }
        public string NombreMaterial { get; set; } = string.Empty;
        public string TipoArchivo { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public int TotalConsultas { get; set; }
        public string? UltimaConsulta { get; set; }
        public double PorcentajeDelTotal { get; set; }
    }
}
