using WorkProject.Contracts.Dtos;

namespace WorkProject.Contracts.Services
{
    public interface IExportService
    {
        byte[] ExportProjectToPdf(int projectId, ExportOptions options);
        void ExportProjectToFile(int projectId, string filePath, ExportOptions options);
    }
}
