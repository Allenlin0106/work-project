using System;

namespace WorkProject.Domain.Entities
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
        public int UploadedByUserId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreatedUtc { get; set; }

        public virtual ProjectTask Task { get; set; }
        public virtual Project Project { get; set; }
        public virtual User UploadedBy { get; set; }
    }
}
