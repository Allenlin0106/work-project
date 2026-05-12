using System.Data.Entity;
using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ProjectPlanningDbContext context) : base(context) { }

        public Project GetWithTasks(int id)
        {
            return Set
                .Include(p => p.Tasks)
                .Include(p => p.Owner)
                .Include(p => p.Members.Select(m => m.User))
                .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Project> ListForUser(int userId)
        {
            return Set
                .Where(p => p.OwnerId == userId || p.Members.Any(m => m.UserId == userId));
        }
    }
}
