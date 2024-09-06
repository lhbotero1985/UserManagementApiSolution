// Infrastructure/Security/BCryptSecurityService.cs
using BCrypt.Net;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Security
{
    public class BCryptSecurityService : ISecurityService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
