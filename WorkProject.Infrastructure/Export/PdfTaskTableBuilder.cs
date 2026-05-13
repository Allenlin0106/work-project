using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using WorkProject.Contracts.Dtos;

namespace WorkProject.Infrastructure.Export
{
    internal static class PdfTaskTableBuilder
    {
        public static void Build(Section section, IReadOnlyList<TaskDto> tasks)
        {
            var heading = section.AddParagraph("Tasks");
            heading.Format.Font.Size = 14;
            heading.Format.Font.Bold = true;
            heading.Format.SpaceAfter = "0.3cm";

            var table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Borders.Color = Colors.Gray;

            table.AddColumn(Unit.FromCentimeter(6));
            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(2.5));
            table.AddColumn(Unit.FromCentimeter(2.5));
            table.AddColumn(Unit.FromCentimeter(1.5));
            table.AddColumn(Unit.FromCentimeter(2));

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Format.Font.Bold = true;
            header.Cells[0].AddParagraph("Title");
            header.Cells[1].AddParagraph("Assignee");
            header.Cells[2].AddParagraph("Start");
            header.Cells[3].AddParagraph("End");
            header.Cells[4].AddParagraph("Progress");
            header.Cells[5].AddParagraph("Status");

            foreach (var task in tasks)
            {
                var row = table.AddRow();
                var indent = task.ParentTaskId.HasValue ? "    " : string.Empty;
                row.Cells[0].AddParagraph(indent + (task.IsMilestone ? "[M] " : string.Empty) + task.Title);
                row.Cells[1].AddParagraph(task.AssignedDisplayName ?? "-");
                row.Cells[2].AddParagraph(task.StartDate.ToString("yyyy-MM-dd"));
                row.Cells[3].AddParagraph(task.EndDate.ToString("yyyy-MM-dd"));
                row.Cells[4].AddParagraph($"{task.ProgressPercent}%");
                row.Cells[5].AddParagraph(task.Status.ToString());
            }
        }

        public static void BuildMembers(Section section, IReadOnlyList<ProjectMemberDto> members)
        {
            var heading = section.AddParagraph("Members");
            heading.Format.Font.Size = 14;
            heading.Format.Font.Bold = true;
            heading.Format.SpaceAfter = "0.3cm";

            var table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Borders.Color = Colors.Gray;

            table.AddColumn(Unit.FromCentimeter(6));
            table.AddColumn(Unit.FromCentimeter(4));
            table.AddColumn(Unit.FromCentimeter(4));

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Format.Font.Bold = true;
            header.Cells[0].AddParagraph("Display Name");
            header.Cells[1].AddParagraph("Role");
            header.Cells[2].AddParagraph("Joined");

            foreach (var m in members)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(m.DisplayName);
                row.Cells[1].AddParagraph(m.Role.ToString());
                row.Cells[2].AddParagraph(m.JoinedUtc.ToString("yyyy-MM-dd"));
            }
        }
    }
}
