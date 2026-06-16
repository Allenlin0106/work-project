using System.Collections.Generic;
using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(ProjectPlanningDbContext context) : base(context) { }

        public IEnumerable<Attachment> GetByProject(int projectId)
        {
            return Set.Where(a => a.ProjectId == projectId).ToList();
        }
    }
}
