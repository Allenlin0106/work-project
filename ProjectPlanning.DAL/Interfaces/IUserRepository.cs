using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByWindowsAccount(string account);
    }
}
