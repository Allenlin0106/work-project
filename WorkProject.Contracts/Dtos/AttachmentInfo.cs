using System;

namespace WorkProject.Contracts.Dtos
{
    public class AttachmentInfo
    {
        public int AttachmentId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
        public int UploadedByUserId { get; set; }
        public string UploadedByDisplayName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
