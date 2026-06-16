using System.Collections.Generic;
using System.IO;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface IAttachmentService
    {
        IEnumerable<Attachment> GetByProject(int projectId);
        int Save(int projectId, int? taskId, string fileName, string contentType, Stream content, string storageRoot);
        void Delete(int attachmentId, string storageRoot);
    }
}
