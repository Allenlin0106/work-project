using ProjectPlanning.Entities.Dtos;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface IGanttService
    {
        GanttDataDto BuildGanttData(int projectId);
    }
}
