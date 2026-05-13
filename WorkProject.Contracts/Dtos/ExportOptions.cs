namespace WorkProject.Contracts.Dtos
{
    public class ExportOptions
    {
        public bool IncludeSummary { get; set; } = true;
        public bool IncludeMembers { get; set; } = true;
        public bool IncludeTasks { get; set; } = true;
        public bool IncludeGantt { get; set; } = true;
        public bool IncludeAuditExcerpt { get; set; } = false;
        public int AuditExcerptCount { get; set; } = 50;
        public byte[] GanttImage { get; set; }
    }
}
