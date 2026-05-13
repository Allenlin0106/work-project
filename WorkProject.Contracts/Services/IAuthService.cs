using WorkProject.Contracts.Dtos;

namespace WorkProject.Contracts.Services
{
    public interface IAuthService
    {
        AuthResult Login(string userName, string password);
        void Logout();
        UserPrincipal CurrentUser { get; }
        void ChangePassword(int userId, string oldPassword, string newPassword);
        int CreateUser(string userName, string displayName, string initialPassword);
    }
}
