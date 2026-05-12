using System;

namespace ProjectPlanning.Entities.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Body { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
