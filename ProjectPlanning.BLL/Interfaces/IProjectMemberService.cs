using System.Collections.Generic;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface IProjectMemberService
    {
        IEnumerable<MemberDto> GetByProject(int projectId);
        int AddMember(int projectId, string windowsAccount, ProjectRole role);
        void RemoveMember(int memberId);
    }
}
