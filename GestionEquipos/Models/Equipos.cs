namespace GestionEquipos.Models
{
    public class Equipos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Precio { get; set; }
        public int IdProveedor { get; set; }
        public bool Activo { get; set; }
    }
}
