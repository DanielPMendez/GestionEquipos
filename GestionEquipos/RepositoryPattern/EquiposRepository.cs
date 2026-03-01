using GestionEquipos.Config;
using GestionEquipos.Models;
using GestionEquipos.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionEquipos.RepositoryPattern
{
    public class EquiposRepository : IEquiposRepository
    {
        private readonly DbContext _context;
        private readonly string SP_NAME = "sp_Equipos_CRUD";

        public EquiposRepository(DbContext context) => _context = context;

        public async Task<List<EquipoDtoGet>> ObtenerTodoAsync()
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_ALL");

            DataTable dt = await _context.SeleccionarAsync(cmd);
            return dt.AsEnumerable().Select(c => new EquipoDtoGet
            {
                Id = Convert.ToInt32(c["Id"]),
                Nombre = c["Nombre"].ToString()!,
                Marca = c["Marca"].ToString()!,
                Modelo = c["Modelo"].ToString()!,
                NumeroSerie = c["NumeroSerie"].ToString()!,
                FechaCompra = Convert.ToDateTime(c["FechaCompra"]),
                Precio = Convert.ToDecimal(c["Precio"]),
                IdProveedor = Convert.ToInt32(c["IdProveedor"]),
                ProveedorNombre = c["ProveedorNombre"].ToString()!,
                Activo = Convert.ToBoolean(c["Activo"])
            }).ToList();
        }

        public async Task<Equipos> ObtenerPorId(int id)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "SELECT_BY_ID");
            cmd.Parameters.AddWithValue("@Id", id);

            DataTable dt = await _context.SeleccionarAsync(cmd);
            if (dt.Rows.Count == 0)
                throw new Exception("Equipo no encontrado");

            var row = dt.Rows[0];
            return new Equipos
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString()!,
                Marca = row["Marca"].ToString()!,
                Modelo = row["Modelo"].ToString()!,
                NumeroSerie = row["NumeroSerie"].ToString()!,
                FechaCompra = Convert.ToDateTime(row["FechaCompra"]),
                Precio = Convert.ToDecimal(row["Precio"]),
                IdProveedor = Convert.ToInt32(row["IdProveedor"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        public async Task<int> InsertarAsync(EquipoDtoIns eIns)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "INSERT");
            cmd.Parameters.AddWithValue("@Nombre", eIns.Nombre);
            cmd.Parameters.AddWithValue("@Marca", eIns.Marca);
            cmd.Parameters.AddWithValue("@Modelo", eIns.Modelo);
            cmd.Parameters.AddWithValue("@NumeroSerie", eIns.NumeroSerie);
            cmd.Parameters.AddWithValue("@FechaCompra", eIns.FechaCompra);
            cmd.Parameters.AddWithValue("@Precio", eIns.Precio);
            cmd.Parameters.AddWithValue("@IdProveedor", eIns.IdProveedor);
            cmd.Parameters.AddWithValue("@Activo", eIns.Activo);

            return await _context.EjecutarAsync(cmd, true);
        }

        public async Task<bool> ActualizarAsync(EquipoDtoUpd eUpd)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "UPDATE");
            cmd.Parameters.AddWithValue("@Id", eUpd.Id);
            cmd.Parameters.AddWithValue("@Nombre", eUpd.Nombre);
            cmd.Parameters.AddWithValue("@Marca", eUpd.Marca);
            cmd.Parameters.AddWithValue("@Modelo", eUpd.Modelo);
            cmd.Parameters.AddWithValue("@NumeroSerie", eUpd.NumeroSerie);
            cmd.Parameters.AddWithValue("@FechaCompra", eUpd.FechaCompra);
            cmd.Parameters.AddWithValue("@Precio", eUpd.Precio);
            cmd.Parameters.AddWithValue("@IdProveedor", eUpd.IdProveedor);
            cmd.Parameters.AddWithValue("@Activo", eUpd.Activo);

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

    public interface IEquiposRepository
    {
        Task<List<EquipoDtoGet>> ObtenerTodoAsync();
        Task<Equipos> ObtenerPorId(int id);
        Task<int> InsertarAsync(EquipoDtoIns eIns);
        Task<bool> ActualizarAsync(EquipoDtoUpd eUpd);
        Task<bool> EliminarAsync(int id);
    }
}
