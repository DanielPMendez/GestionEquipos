using GestionEquipos.Models;
using GestionEquipos.Models.DTOs;
using GestionEquipos.RepositoryPattern;

namespace GestionEquipos.ServiceLayer
{
    public class EquipoService : IEquipoService
    {
        private readonly IEquiposRepository _repository;

        public EquipoService(IEquiposRepository repository) => _repository = repository;

        public async Task<List<EquipoDtoGet>> ObtenerTodoAsync()
        {
            var equipos = await _repository.ObtenerTodoAsync();
            if (equipos == null)
            {
                throw new ArgumentException("No se encontraron equipos.");
            }
            return equipos;
        }

        public async Task<Equipos> ObtenerPorId(int id)
        {
            var equipo = await _repository.ObtenerPorId(id);
            if (equipo == null)
            {
                throw new KeyNotFoundException($"No se encontró el equipo con ID {id}.");
            }
            return equipo;
        }

        public async Task<int> InsertarAsync(EquipoDtoIns eIns)
        {
            if (eIns == null)
            {
                throw new ArgumentNullException(nameof(eIns), "El equipo a insertar no puede ser nulo.");
            }

            int newId = await _repository.InsertarAsync(eIns);
            if (newId <= 0)
            {
                throw new InvalidOperationException("No se pudo insertar el equipo.");
            }
            return newId;
        }

        public async Task<bool> ActualizarAsync(EquipoDtoUpd eUpd)
        {
            if (eUpd == null)
            {
                throw new ArgumentNullException(nameof(eUpd), "El equipo a actualizar no puede ser nulo.");
            }
            bool success = await _repository.ActualizarAsync(eUpd);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo actualizar el equipo con ID {eUpd.Id}.");
            }
            return success;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            bool success = await _repository.EliminarAsync(id);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo eliminar el equipo con ID {id}.");
            }
            return success;
        }
    }

    public interface IEquipoService
    {
        Task<List<EquipoDtoGet>> ObtenerTodoAsync();
        Task<Equipos> ObtenerPorId(int id);
        Task<int> InsertarAsync(EquipoDtoIns eIns);
        Task<bool> ActualizarAsync(EquipoDtoUpd eUpd);
        Task<bool> EliminarAsync(int id);
    }
}
