using System.Collections.Generic;
using WorkProject.Contracts.Dtos;
using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Services
{
    public interface IProjectService
    {
        int CreateProject(NewProjectDto dto);
        void UpdateProject(ProjectDto dto);
        void DeleteProject(int projectId);
        ProjectDto Get(int projectId);
        IReadOnlyList<ProjectSummaryDto> ListForCurrentUser();
        void AddMember(int projectId, int userId, ProjectRole role);
        void RemoveMember(int projectMemberId);
    }
}
