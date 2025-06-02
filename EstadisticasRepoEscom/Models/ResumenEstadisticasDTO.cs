namespace EstadisticasRepoEscom.Models
{
    public class ResumenEstadisticasDTO
    {
        public int TotalConsultasHistorico { get; set; }
        public int TotalCarrerasConsultadas { get; set; }
        public int TotalSemestresConsultados { get; set; }
        public int TotalMateriasConsultadas { get; set; }
        public int TotalAutoresConsultados { get; set; }
        public int TotalMaterialesConsultados { get; set; }
        public string? CarreraMasConsultada { get; set; }
        public string? AutorMasConsultado { get; set; }
        public string? MaterialMasConsultado { get; set; }
    }
}
