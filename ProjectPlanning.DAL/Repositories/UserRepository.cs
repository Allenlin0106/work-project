using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ProjectPlanningDbContext context) : base(context) { }

        public User GetByWindowsAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account)) return null;
            return Set.FirstOrDefault(u => u.WindowsAccount == account);
        }
    }
}
