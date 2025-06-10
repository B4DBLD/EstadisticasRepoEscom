namespace EstadisticasRepoEscom.Models
{
    public class EstadisticaMaterialesPorAutorDTO
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int CantidadMateriales { get; set; }
        public double PorcentajeDelTotal { get; set; }
    }
}