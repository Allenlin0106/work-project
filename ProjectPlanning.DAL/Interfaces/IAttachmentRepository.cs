using System.Collections.Generic;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        IEnumerable<Attachment> GetByProject(int projectId);
    }
}
