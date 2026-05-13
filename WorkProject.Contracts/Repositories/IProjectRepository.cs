using System.Collections.Generic;
using WorkProject.Domain.Entities;

namespace WorkProject.Contracts.Repositories
{
    public interface IProjectRepository
    {
        Project GetById(int projectId, bool includeMembers = false);
        IReadOnlyList<Project> GetForUser(int userId);
        IReadOnlyList<Project> GetAll();
        void Add(Project project);
        void Update(Project project);
        void Delete(int projectId);
        void AddMember(ProjectMember member);
        void RemoveMember(int projectMemberId);
        ProjectMember GetMember(int projectMemberId);
    }
}
