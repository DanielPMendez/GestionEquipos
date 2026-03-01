namespace GestionEquipos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RolNombre { get; set; }
        public int IdRol { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
