using System;

namespace WorkProject.Contracts.Dtos
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
        public int AuthorUserId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Body { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
