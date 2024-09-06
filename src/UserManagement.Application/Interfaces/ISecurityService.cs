// Application/Interfaces/ISecurityService.cs
namespace UserManagement.Application.Interfaces
{
    public interface ISecurityService
    {
        string HashPassword(string password);
    }
}
