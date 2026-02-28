using GestionEquipos.Config;
using GestionEquipos.RepositoryPattern;
using System.Data;
using System.Text;

namespace GestionEquipos.ServiceLayer
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtService _jwt;

        public AuthService(IAuthRepository repository, ILogger<AuthService> logger, JwtService jwt)
        {
            _repository = repository;
            _logger = logger;
            _jwt = jwt;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            DataTable dt = await _repository.LoginAsync(username);

            if (dt.Rows.Count == 0)
            {
                _logger.LogWarning("Intento de login fallido: Usuario {Username} no existe", username);
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            var row = dt.Rows[0];

            byte[] hashBytes = (byte[])row["PasswordHash"];
            string storedHash = Encoding.UTF8.GetString(hashBytes);

            if (!BCrypt.Net.BCrypt.Verify(password, storedHash))
            {
                _logger.LogWarning("Intento de login fallido: Password incorrecto para {Username}", username);
                throw new UnauthorizedAccessException("Credenciales incorrectas.");
            }

            _logger.LogInformation("Usuario {Username} inició sesión correctamente", username);

            return _jwt.GenerarToken(
                (int)row["Id"],
                row["Username"].ToString()!,
                row["RolNombre"].ToString()!
            );
        }

        public async Task<int> RegistrarUsuarioAsync(string username, string password, int idRol)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("El nombre de usuario y la contraseña no pueden estar vacíos.");
            }
            byte[] passwordHash = Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.HashPassword(password));
            return await _repository.RegistrarUsuarioAsync(username, passwordHash, idRol);
        }
    }

    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task<int> RegistrarUsuarioAsync(string username, string password, int idRol);
    }
}
