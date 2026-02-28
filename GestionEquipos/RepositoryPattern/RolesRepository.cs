using GestionEquipos.Config;
using GestionEquipos.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionEquipos.RepositoryPattern
{
    public class RolesRepository : IRolesRepository
    {
        private readonly DbContext _context;
        private const string SP_NAME = "sp_Roles_CRUD";

        public RolesRepository(DbContext context) => _context = context;

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_ALL");

            DataTable dt = await _context.SeleccionarAsync(cmd);
            return dt.AsEnumerable().Select(row => new Rol
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString()!
            }).ToList();
        }

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_BY_ID");
            cmd.Parameters.AddWithValue("@Id", id);

            DataTable dt = await _context.SeleccionarAsync(cmd);
            if (dt.Rows.Count == 0) return null;

            return new Rol
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Nombre = dt.Rows[0]["Nombre"].ToString()!
            };
        }

        public async Task<int> InsertarAsync(string nombre)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "INSERT");
            cmd.Parameters.AddWithValue("@Nombre", nombre);

            return await _context.EjecutarAsync(cmd, true);
        }

        public async Task<bool> ActualizarAsync(int id, string nombre)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "UPDATE");
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Nombre", nombre);

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

    public interface IRolesRepository
    {
        Task<List<Rol>> ObtenerTodosAsync();
        Task<Rol?> ObtenerPorIdAsync(int id);
        Task<int> InsertarAsync(string nombre);
        Task<bool> ActualizarAsync(int id, string nombre);
        Task<bool> EliminarAsync(int id);
    }
}
