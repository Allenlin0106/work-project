using System;

namespace WorkProject.Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
        public int AuthorUserId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedUtc { get; set; }

        public virtual ProjectTask Task { get; set; }
        public virtual Project Project { get; set; }
        public virtual User Author { get; set; }
    }
}
