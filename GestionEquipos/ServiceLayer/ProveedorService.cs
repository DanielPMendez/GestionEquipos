using GestionEquipos.Models;
using GestionEquipos.Models.DTOs;
using GestionEquipos.RepositoryPattern;

namespace GestionEquipos.ServiceLayer
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;

        public ProveedorService(IProveedorRepository repository) => _repository = repository;

        public async Task<List<Proveedores>> ObtenerTodoAsync()
        {
            var proveedores = await _repository.ObtenerTodoAsync();
            if (proveedores == null)
            {
                throw new KeyNotFoundException("No se pudieron obtener los proveedores.");
            }
            return proveedores;
        }

        public async Task<Proveedores> ObtenerPorId(int id)
        {
            var proveedor = await _repository.ObtenerPorId(id);
            if (proveedor == null)
            {
                throw new KeyNotFoundException($"No se encontró el proveedor con ID {id}.");
            }
            return proveedor;
        }

        public async Task<int> InsertarAsync(ProveedorDtoIns pIns)
        {
            if (pIns == null)
            {
                throw new ArgumentNullException(nameof(pIns), "El proveedor a insertar no puede ser nulo.");
            }
            int success = await _repository.InsertarAsync(pIns);
            if (success == 0)
            {
                throw new InvalidOperationException("No se pudo insertar el nuevo proveedor.");
            }
            return success;
        }

        public async Task<bool> ActualizarAsync(ProveedorDtoUpd pUpd)
        {
            if (pUpd == null)
            {
                throw new ArgumentNullException(nameof(pUpd), "El proveedor a actualizar no puede ser nulo.");
            }
            bool success = await _repository.ActualizaAsync(pUpd);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo actualizar el proveedor con ID {pUpd.Id}.");
            }
            return success;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            bool success = await _repository.EliminarAsync(id);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo eliminar el proveedor con ID {id}.");
            }
            return success;
        }
    }

    public interface IProveedorService
    {
        Task<List<Proveedores>> ObtenerTodoAsync();
        Task<Proveedores> ObtenerPorId(int id);
        Task<int> InsertarAsync(ProveedorDtoIns pIns);
        Task<bool> ActualizarAsync(ProveedorDtoUpd pUpd);
        Task<bool> EliminarAsync(int id);
    }
}
