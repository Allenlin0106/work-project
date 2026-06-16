using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(ProjectPlanningDbContext context) : base(context) { }

        public IEnumerable<ProjectMember> GetByProject(int projectId)
        {
            return Set.Include(m => m.User).Where(m => m.ProjectId == projectId).ToList();
        }

        public ProjectMember Find(int projectId, int userId)
        {
            return Set.FirstOrDefault(m => m.ProjectId == projectId && m.UserId == userId);
        }
    }
}
