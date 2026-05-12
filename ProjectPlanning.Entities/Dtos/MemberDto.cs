using System;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string WindowsAccount { get; set; }
        public string DisplayName { get; set; }
        public ProjectRole Role { get; set; }
        public DateTime AddedUtc { get; set; }
    }
}
