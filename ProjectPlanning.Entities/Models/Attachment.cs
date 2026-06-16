using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanning.Entities.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int? TaskId { get; set; }
        public virtual TaskItem Task { get; set; }

        [Required, StringLength(260)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public long SizeBytes { get; set; }

        [Required, StringLength(400)]
        public string StoragePath { get; set; }

        public int UploadedById { get; set; }
        public virtual User UploadedBy { get; set; }

        public DateTime UploadedUtc { get; set; }
    }
}
