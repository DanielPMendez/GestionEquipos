using GestionEquipos.Models;
using GestionEquipos.RepositoryPattern;
using System.Text;

namespace GestionEquipos.ServiceLayer
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository) => _repository = repository;

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            var usuarios = await _repository.ObtenerTodosAsync();
            if (usuarios == null)
            {
                throw new KeyNotFoundException("No se pudieron obtener los usuarios.");
            }
            return usuarios;
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            var usuario = await _repository.ObtenerPorId(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}.");
            }
            return usuario;
        }

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }
            bool success = await _repository.ActualizarAsync(usuario);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo actualizar el usuario con ID {usuario.Id}.");
            }
            return success;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            bool success = await _repository.EliminarAsync(id);
            if (!success)
            {
                throw new InvalidOperationException($"No se pudo eliminar el usuario con ID {id}.");
            }
            return success;
        }
    }

    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario> ObtenerPorId(int id);
        Task<bool> ActualizarAsync(Usuario usuario);
        Task<bool> EliminarAsync(int id);
    }
}
