using System;
using WorkProject.Contracts.Dtos;

namespace WorkProject.Business.Validation
{
    public static class TaskValidator
    {
        public static void Validate(NewTaskDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.ProjectId <= 0)
                throw new InvalidOperationException("Task must belong to a project.");
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new InvalidOperationException("Task title is required.");
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("Task end date cannot be earlier than start date.");
        }

        public static void Validate(TaskDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new InvalidOperationException("Task title is required.");
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("Task end date cannot be earlier than start date.");
            if (dto.ProgressPercent < 0 || dto.ProgressPercent > 100)
                throw new InvalidOperationException("Task progress must be between 0 and 100.");
        }
    }
}
