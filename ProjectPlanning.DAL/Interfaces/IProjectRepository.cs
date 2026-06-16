using System.Linq;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Project GetWithTasks(int id);
        IQueryable<Project> ListForUser(int userId);
    }
}
