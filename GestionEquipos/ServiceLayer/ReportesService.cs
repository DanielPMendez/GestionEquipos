using GestionEquipos.Models;
using GestionEquipos.Models.DTOs;
using GestionEquipos.RepositoryPattern;

namespace GestionEquipos.ServiceLayer
{
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository _repository;

        public ReportesService(IReportesRepository repository) => _repository = repository;

        public Task<PagedResponse<ReporteEquipoCategoriaDto>> ObtenerEquiposPorCategoriaAsync(
            int page = 1,
            int pageSize = 10,
            string? nombre = null,
            string? marca = null,
            int? idProveedor = null,
            string sortBy = "Nombre",
            string sortDir = "ASC")
        {
            var columnasValidas = new[] { "Nombre", "Marca", "Modelo", "Precio", "Id" };
            if (!columnasValidas.Contains(sortBy))
            {
                throw new ArgumentException($"La columna de ordenamiento '{sortBy}' no es válida.");
            }

            return _repository.ObtenerEquiposPorCategoriaAsync(page, pageSize, nombre, marca, idProveedor, sortBy, sortDir);
        }

        public Task<PagedResponse<ReporteEquipoRentabilidadDto>> ObtenerEquiposTopRentabilidadAsync(
            int page = 1,
            int pageSize = 10,
            decimal? minPrecio = null,
            decimal? maxPrecio = null,
            string sortBy = "Precio",
            string sortDir = "DESC")
        {
            var columnasValidas = new[] { "Nombre", "Marca", "Modelo", "Precio", "Id" };
            if (!columnasValidas.Contains(sortBy))
            {
                throw new ArgumentException($"La columna de ordenamiento '{sortBy}' no es válida.");
            }

            return _repository.ObtenerEquiposTopRentabilidadAsync(page, pageSize, minPrecio, maxPrecio, sortBy, sortDir);
        }
    }

    public interface IReportesService
    {
        Task<PagedResponse<ReporteEquipoCategoriaDto>> ObtenerEquiposPorCategoriaAsync(
            int page = 1,
            int pageSize = 10,
            string? nombre = null,
            string? marca = null,
            int? idProveedor = null,
            string sortBy = "Nombre",
            string sortDir = "ASC");

        Task<PagedResponse<ReporteEquipoRentabilidadDto>> ObtenerEquiposTopRentabilidadAsync(
            int page = 1,
            int pageSize = 10,
            decimal? minPrecio = null,
            decimal? maxPrecio = null,
            string sortBy = "Precio",
            string sortDir = "DESC");
    }
}
