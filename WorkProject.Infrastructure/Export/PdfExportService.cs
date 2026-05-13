using System;
using System.IO;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;

namespace WorkProject.Infrastructure.Export
{
    public class PdfExportService : IExportService
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly IAuditService _auditService;

        public PdfExportService(IProjectService projectService, ITaskService taskService,
            IAuthService authService, IAuditService auditService)
        {
            _projectService = projectService;
            _taskService = taskService;
            _authService = authService;
            _auditService = auditService;
        }

        public byte[] ExportProjectToPdf(int projectId, ExportOptions options)
        {
            options = options ?? new ExportOptions();
            var project = _projectService.Get(projectId);
            if (project == null) throw new InvalidOperationException("Project not found.");

            var tasks = _taskService.ListByProject(projectId);
            var generatedBy = _authService.CurrentUser?.DisplayName ?? "system";

            var document = new Document();
            document.Info.Title = project.Name;
            document.Info.Author = generatedBy;
            document.DefaultPageSetup.PageFormat = PageFormat.A4;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(2);
            document.DefaultPageSetup.RightMargin = Unit.FromCentimeter(2);
            document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(2);
            document.DefaultPageSetup.BottomMargin = Unit.FromCentimeter(2);

            var section = document.AddSection();
            PdfCoverBuilder.Build(section, project, generatedBy);

            if (options.IncludeSummary)
            {
                section.AddPageBreak();
                BuildSummary(section, tasks);
            }

            if (options.IncludeMembers && project.Members.Count > 0)
            {
                section.AddParagraph().Format.SpaceBefore = "0.6cm";
                PdfTaskTableBuilder.BuildMembers(section, project.Members);
            }

            if (options.IncludeTasks)
            {
                section.AddPageBreak();
                PdfTaskTableBuilder.Build(section, tasks);
            }

            var renderer = new PdfDocumentRenderer(unicode: true) { Document = document };
            renderer.RenderDocument();

            if (options.IncludeGantt && options.GanttImage != null && options.GanttImage.Length > 0)
            {
                AppendGanttPage(renderer.PdfDocument, options.GanttImage);
            }

            using (var ms = new MemoryStream())
            {
                renderer.PdfDocument.Save(ms, closeStream: false);
                var bytes = ms.ToArray();
                _auditService.RecordExport(_authService.CurrentUser?.UserId ?? 0,
                    $"Project:{projectId}");
                return bytes;
            }
        }

        public void ExportProjectToFile(int projectId, string filePath, ExportOptions options)
        {
            var bytes = ExportProjectToPdf(projectId, options);
            File.WriteAllBytes(filePath, bytes);
        }

        private void BuildSummary(Section section, System.Collections.Generic.IReadOnlyList<TaskDto> tasks)
        {
            var heading = section.AddParagraph("Summary");
            heading.Format.Font.Size = 14;
            heading.Format.Font.Bold = true;
            heading.Format.SpaceAfter = "0.3cm";

            var total = tasks.Count;
            var completed = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Completed);
            var inProgress = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.InProgress);
            var overdue = tasks.Count(t => t.EndDate < DateTime.UtcNow.Date &&
                                            t.Status != Domain.Enums.TaskStatus.Completed);
            var avg = total == 0 ? 0 : tasks.Average(t => t.ProgressPercent);

            section.AddParagraph($"Total tasks: {total}");
            section.AddParagraph($"Completed: {completed}");
            section.AddParagraph($"In progress: {inProgress}");
            section.AddParagraph($"Overdue: {overdue}");
            section.AddParagraph($"Average completion: {avg:F1}%");
        }

        private void AppendGanttPage(PdfDocument pdf, byte[] imageBytes)
        {
            var page = pdf.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Landscape;
            using (var gfx = XGraphics.FromPdfPage(page))
            using (var ms = new MemoryStream(imageBytes))
            {
                var image = XImage.FromStream(ms);
                var marginPt = 40;
                var maxWidth = page.Width - 2 * marginPt;
                var maxHeight = page.Height - 2 * marginPt;
                var ratio = Math.Min(maxWidth / image.PixelWidth, maxHeight / image.PixelHeight);
                var drawWidth = image.PixelWidth * ratio;
                var drawHeight = image.PixelHeight * ratio;
                gfx.DrawImage(image, marginPt, marginPt, drawWidth, drawHeight);

                var title = new XFont("Verdana", 12, XFontStyle.Bold);
                gfx.DrawString("Gantt Snapshot", title, XBrushes.Black,
                    new XRect(0, 10, page.Width, 30), XStringFormats.Center);
            }
        }
    }
}
