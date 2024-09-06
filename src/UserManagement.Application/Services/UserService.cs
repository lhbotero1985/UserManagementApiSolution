using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using BCrypt.Net;
namespace UserManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync() => await _userRepository.GetUsersAsync();

        public async Task<User?> GetUserAsync(int id) => await _userRepository.GetUserAsync(id);

        public async Task AddUserAsync(User user) => await _userRepository.AddUserAsync(user);

        public async Task UpdateUserAsync(User user) => await _userRepository.UpdateUserAsync(user);

        public async Task DeleteUserAsync(int id) => await _userRepository.DeleteUserAsync(id);

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // Implementa la lógica para validar las credenciales del usuario
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(user.Password, password))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            // Implementa la lógica para verificar la contraseña (por ejemplo, usando un hash)
            return hashedPassword == password; // Simplificado, usa hashing en producción
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
