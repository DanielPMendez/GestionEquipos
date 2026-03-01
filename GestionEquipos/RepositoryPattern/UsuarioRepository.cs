using GestionEquipos.Config;
using GestionEquipos.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace GestionEquipos.RepositoryPattern
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbContext _context;
        private readonly string SP_NAME = "sp_Usuarios_Admin";

        public UsuarioRepository(DbContext context) => _context = context;

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_ALL");

            DataTable dt = await _context.SeleccionarAsync(cmd);
            return dt.AsEnumerable().Select(row => new Usuario
            {
                Id = Convert.ToInt32(row["Id"]),
                UserName = row["UserName"].ToString(),
                RolNombre = row["RolNombre"].ToString(),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"])
            }).ToList();
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_BY_ID");
            cmd.Parameters.AddWithValue("@Id", id);

            DataTable dt = await _context.SeleccionarAsync(cmd);
            if (dt.Rows.Count == 0) return null;

            return new Usuario
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                UserName = dt.Rows[0]["UserName"].ToString(),
                IdRol = Convert.ToInt32(dt.Rows[0]["IdRol"])
            };
        }

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "UPDATE");
            cmd.Parameters.AddWithValue("Username", usuario.UserName);
            cmd.Parameters.AddWithValue("IdRol", usuario.IdRol);
            cmd.Parameters.AddWithValue("Id", usuario.Id);

            return await _context.EjecutarAsync(cmd, true) > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "DELETE");
            cmd.Parameters.AddWithValue("@Id", id);

            return await _context.EjecutarAsync(cmd, true) > 0;
        }
    }

    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario> ObtenerPorId(int id);
        Task<bool> ActualizarAsync(Usuario usuario);
        Task<bool> EliminarAsync(int id);
    }
}
