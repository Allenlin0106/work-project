using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanning.Entities.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(256)]
        public string WindowsAccount { get; set; }

        [StringLength(200)]
        public string DisplayName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedUtc { get; set; }

        public virtual ICollection<ProjectMember> Memberships { get; set; }
    }
}
