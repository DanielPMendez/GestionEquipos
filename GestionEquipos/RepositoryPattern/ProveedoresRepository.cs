using GestionEquipos.Config;
using GestionEquipos.Models;
using GestionEquipos.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionEquipos.RepositoryPattern
{
    public class ProveedoresRepository : IProveedorRepository
    {
        private readonly DbContext _context;
        private readonly string SP_NAME = "sp_Proveedores_CRUD";

        public ProveedoresRepository(DbContext context) => _context = context;

        public async Task<List<Proveedores>> ObtenerTodoAsync()
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_ALL");
            DataTable dt = await _context.SeleccionarAsync(cmd);

            return dt.AsEnumerable().Select(row => new Proveedores
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString()!,
                RUC_NIT = row["RUC_NIT"].ToString()!,
                Telefono = row["Telefono"].ToString()!,
                Email = row["Email"].ToString()!,
                Direccion = row["Direccion"].ToString()!,
                Activo = Convert.ToBoolean(row["Activo"])
            }).ToList();
        }

        public async Task<Proveedores> ObtenerPorId(int id)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_BY_ID");
            cmd.Parameters.AddWithValue("@Id", id);

            DataTable dt = await _context.SeleccionarAsync(cmd);
            if (dt.Rows.Count == 0) return null;

            return new Proveedores
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Nombre = dt.Rows[0]["Nombre"].ToString()!,
                RUC_NIT = dt.Rows[0]["RUC_NIT"].ToString()!,
                Telefono = dt.Rows[0]["Telefono"].ToString()!,
                Email = dt.Rows[0]["Email"].ToString()!,
                Direccion = dt.Rows[0]["Direccion"].ToString()!,
                Activo = Convert.ToBoolean(dt.Rows[0]["Activo"])
            };
        }

        public async Task<int> InsertarAsync(ProveedorDtoIns pIns)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "INSERT");
            cmd.Parameters.AddWithValue("@Nombre", pIns.Nombre);
            cmd.Parameters.AddWithValue("@RUC_NIT", pIns.RUC_NIT);
            cmd.Parameters.AddWithValue("@Telefono", pIns.Telefono);
            cmd.Parameters.AddWithValue("@Email", pIns.Email);
            cmd.Parameters.AddWithValue("@Direccion", pIns.Direccion);
            cmd.Parameters.AddWithValue("@Activo", pIns.Activo);

            return await _context.EjecutarAsync(cmd, true);
        }

        public async Task<bool> ActualizaAsync(ProveedorDtoUpd pUpd)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "UPDATE");
            cmd.Parameters.AddWithValue("@Nombre", pUpd.Nombre);
            cmd.Parameters.AddWithValue("@RUC_NIT", pUpd.RUC_NIT);
            cmd.Parameters.AddWithValue("@Telefono", pUpd.Telefono);
            cmd.Parameters.AddWithValue("@Email", pUpd.Email);
            cmd.Parameters.AddWithValue("@Direccion", pUpd.Direccion);
            cmd.Parameters.AddWithValue("@Activo", pUpd.Activo);
            cmd.Parameters.AddWithValue("@Id", pUpd.Id);

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

    public interface IProveedorRepository
    {
        Task<List<Proveedores>> ObtenerTodoAsync();
        Task<Proveedores> ObtenerPorId(int id);
        Task<int> InsertarAsync(ProveedorDtoIns pIns);
        Task<bool> ActualizaAsync(ProveedorDtoUpd pUpd);
        Task<bool> EliminarAsync(int id);
    }
}
