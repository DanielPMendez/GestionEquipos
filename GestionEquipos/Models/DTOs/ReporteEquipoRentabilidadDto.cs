namespace GestionEquipos.Models.DTOs
{
    public class ReporteEquipoRentabilidadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
    }
}