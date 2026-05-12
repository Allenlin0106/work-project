using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanning.Entities.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int? TaskId { get; set; }
        public virtual TaskItem Task { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        [Required, StringLength(4000)]
        public string Body { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}
