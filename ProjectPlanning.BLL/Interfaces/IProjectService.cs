using System.Collections.Generic;
using ProjectPlanning.Entities.Dtos;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectListDto> ListForCurrentUser();
        ProjectDetailDto Get(int id);
        int Create(ProjectEditDto dto);
        void Update(ProjectEditDto dto);
        void Delete(int id);
    }
}
