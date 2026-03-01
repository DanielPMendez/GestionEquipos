namespace GestionEquipos.Models.DTOs
{
    public class ReporteEquipoCategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string ProveedorNombre { get; set; } = string.Empty;
    }
}