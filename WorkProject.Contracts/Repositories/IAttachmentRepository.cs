using System.Collections.Generic;
using WorkProject.Contracts.Dtos;
using WorkProject.Domain.Entities;

namespace WorkProject.Contracts.Repositories
{
    public interface IAttachmentRepository
    {
        void Add(Attachment attachment);
        Attachment GetById(int attachmentId);
        IReadOnlyList<AttachmentInfo> ListByTask(int taskId);
        IReadOnlyList<AttachmentInfo> ListByProject(int projectId);
        void Delete(int attachmentId);
    }
}
