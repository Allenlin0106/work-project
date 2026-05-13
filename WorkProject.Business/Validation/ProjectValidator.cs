using System;
using WorkProject.Contracts.Dtos;

namespace WorkProject.Business.Validation
{
    public static class ProjectValidator
    {
        public static void Validate(NewProjectDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("Project name is required.");
            if (dto.Name.Length > 200)
                throw new InvalidOperationException("Project name must be 200 characters or fewer.");
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("Project end date cannot be earlier than start date.");
        }

        public static void Validate(ProjectDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("Project name is required.");
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("Project end date cannot be earlier than start date.");
        }
    }
}
