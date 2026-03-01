using GestionEquipos.Config;
using GestionEquipos.Models;
using GestionEquipos.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionEquipos.RepositoryPattern
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly DbContext _context;

        public ReportesRepository(DbContext context) => _context = context;

        public async Task<PagedResponse<ReporteEquipoCategoriaDto>> ObtenerEquiposPorCategoriaAsync(
            int page = 1,
            int pageSize = 10,
            string? nombre = null,
            string? marca = null,
            int? idProveedor = null,
            string sortBy = "Nombre",
            string sortDir = "ASC")
        {
            using var cmd = new SqlCommand("sp_Reporte_Equipos_Categoria");
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@Nombre", (object?)nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Marca", (object?)marca ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdProveedor", (object?)idProveedor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortBy", sortBy);
            cmd.Parameters.AddWithValue("@SortDir", sortDir);

            var (totalCount, dt) = await _context.SeleccionarMultipleAsync(cmd);

            var list = dt.AsEnumerable().Select(r => new ReporteEquipoCategoriaDto
            {
                Id = r.Field<int>("Id"),
                Nombre = r.Field<string>("Nombre") ?? string.Empty,
                Marca = r.Field<string>("Marca") ?? string.Empty,
                Modelo = r.Field<string>("Modelo") ?? string.Empty,
                Precio = r.Field<decimal>("Precio"),
                ProveedorNombre = r.Table.Columns.Contains("ProveedorNombre") ? (r.Field<string>("ProveedorNombre") ?? string.Empty) : string.Empty
            }).ToList();

            return new PagedResponse<ReporteEquipoCategoriaDto>
            {
                Data = list,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResponse<ReporteEquipoRentabilidadDto>> ObtenerEquiposTopRentabilidadAsync(
            int page = 1,
            int pageSize = 10,
            decimal? minPrecio = null,
            decimal? maxPrecio = null,
            string sortBy = "Precio",
            string sortDir = "DESC")
        {
            using var cmd = new SqlCommand("sp_Reporte_Equipos_Rentabilidad");
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@MinPrecio", (object?)minPrecio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaxPrecio", (object?)maxPrecio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortBy", sortBy);
            cmd.Parameters.AddWithValue("@SortDir", sortDir);

            var (totalCount, dt) = await _context.SeleccionarMultipleAsync(cmd);

            var list = dt.AsEnumerable().Select(r => new ReporteEquipoRentabilidadDto
            {
                Id = r.Field<int>("Id"),
                Nombre = r.Field<string>("Nombre") ?? string.Empty,
                Marca = r.Field<string>("Marca") ?? string.Empty,
                Precio = r.Field<decimal>("Precio"),
                NumeroSerie = r.Table.Columns.Contains("NumeroSerie") ? (r.Field<string>("NumeroSerie") ?? string.Empty) : string.Empty
            }).ToList();

            return new PagedResponse<ReporteEquipoRentabilidadDto>
            {
                Data = list,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }

    public interface IReportesRepository
    {
        Task<PagedResponse<ReporteEquipoCategoriaDto>> ObtenerEquiposPorCategoriaAsync(int page = 1, int pageSize = 10, string? nombre = null, string? marca = null, int? idProveedor = null, string sortBy = "Nombre", string sortDir = "ASC");
        Task<PagedResponse<ReporteEquipoRentabilidadDto>> ObtenerEquiposTopRentabilidadAsync(int page = 1, int pageSize = 10, decimal? minPrecio = null, decimal? maxPrecio = null, string sortBy = "Precio", string sortDir = "DESC");
    }
}
