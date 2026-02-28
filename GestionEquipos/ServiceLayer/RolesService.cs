using GestionEquipos.Models;
using GestionEquipos.RepositoryPattern;

namespace GestionEquipos.ServiceLayer
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _repository;

        public RolesService(IRolesRepository repository) => _repository = repository;

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            var roles = await _repository.ObtenerTodosAsync();
            if (roles == null)
            {
                throw new KeyNotFoundException("No se pudieron obtener los roles.");
            }
            return roles;
        } 

        public async Task<Rol?> ObtenerPorIdAsync(int id)
        {
            var rol = await _repository.ObtenerPorIdAsync(id);
            if (rol == null)
            {
                throw new KeyNotFoundException($"No se encontró el rol con ID {id}.");
            }
            return rol;
        }

        public async Task<int> InsertarAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del rol no puede estar vacío.");
            }

            int newId = await _repository.InsertarAsync(nombre);
            if (newId <= 0)
            {
                throw new InvalidOperationException("No se pudo insertar el nuevo rol.");
            }
            return newId;
        }

        public async Task<bool> ActualizarAsync(int id, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del rol no puede estar vacío.");
            }

            bool success = await _repository.ActualizarAsync(id, nombre);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo actualizar el rol con ID {id}.");
            }
            return success;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            bool success = await _repository.EliminarAsync(id);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo eliminar el rol con ID {id}.");
            }
            return success;
        }
    }

    public interface IRolesService
    {
        Task<List<Rol>> ObtenerTodosAsync();
        Task<Rol?> ObtenerPorIdAsync(int id);
        Task<int> InsertarAsync(string nombre);
        Task<bool> ActualizarAsync(int id, string nombre);
        Task<bool> EliminarAsync(int id);
    }
}
