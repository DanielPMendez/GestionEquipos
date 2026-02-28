using GestionEquipos.Config;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace GestionEquipos.RepositoryPattern
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DbContext _context;
        private const string SP_NAME = "sp_Auth";

        public AuthRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<DataTable> LoginAsync(string username)
        {
            using var cmd = new SqlCommand(SP_NAME);
            cmd.Parameters.AddWithValue("@Accion", "LOGIN");
            cmd.Parameters.AddWithValue("@Username", username);

            return await _context.SeleccionarAsync(cmd);
        }

        public async Task<int> RegistrarUsuarioAsync(string username, byte[] passwordHash, int idRol)
        {
            using var cmd = new SqlCommand("sp_Auth");
            cmd.Parameters.AddWithValue("@Accion", "REGISTER");
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.Parameters.AddWithValue("@IdRol", idRol);

            return await _context.EjecutarAsync(cmd, true);
        }
    }

    public interface IAuthRepository
    {
        Task<DataTable> LoginAsync(string username);
        Task<int> RegistrarUsuarioAsync(string username, byte[] passwordHash, int idRol);
    }
}
