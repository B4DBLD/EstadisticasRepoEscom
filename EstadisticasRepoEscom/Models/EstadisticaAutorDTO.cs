namespace EstadisticasRepoEscom.Models
{
    public class EstadisticaAutorDTO
    {
        public int AutorId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalConsultas { get; set; }
        public string? UltimaConsulta { get; set; }
        public double PorcentajeDelTotal { get; set; }
    }
}
