using GestionEquipos.RepositoryPattern;
using System.Text;

namespace GestionEquipos.ServiceLayer
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository) => _repository = repository;

        
    }

    public interface IUsuarioService
    {
    }
}
