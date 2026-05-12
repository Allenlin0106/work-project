using System.Collections.Generic;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        IEnumerable<ProjectMember> GetByProject(int projectId);
        ProjectMember Find(int projectId, int userId);
    }
}
