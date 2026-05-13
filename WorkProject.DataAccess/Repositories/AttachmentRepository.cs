using System.Collections.Generic;
using System.Linq;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly WorkProjectDbContext _context;

        public AttachmentRepository(WorkProjectDbContext context)
        {
            _context = context;
        }

        public void Add(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
        }

        public Attachment GetById(int attachmentId)
        {
            return _context.Attachments.Find(attachmentId);
        }

        public IReadOnlyList<AttachmentInfo> ListByTask(int taskId)
        {
            return _context.Attachments
                .Where(a => a.TaskId == taskId)
                .Select(a => new AttachmentInfo
                {
                    AttachmentId = a.AttachmentId,
                    TaskId = a.TaskId,
                    ProjectId = a.ProjectId,
                    UploadedByUserId = a.UploadedByUserId,
                    UploadedByDisplayName = a.UploadedBy.DisplayName,
                    FileName = a.FileName,
                    ContentType = a.ContentType,
                    Size = a.Content.LongLength,
                    CreatedUtc = a.CreatedUtc
                })
                .ToList();
        }

        public IReadOnlyList<AttachmentInfo> ListByProject(int projectId)
        {
            return _context.Attachments
                .Where(a => a.ProjectId == projectId)
                .Select(a => new AttachmentInfo
                {
                    AttachmentId = a.AttachmentId,
                    TaskId = a.TaskId,
                    ProjectId = a.ProjectId,
                    UploadedByUserId = a.UploadedByUserId,
                    UploadedByDisplayName = a.UploadedBy.DisplayName,
                    FileName = a.FileName,
                    ContentType = a.ContentType,
                    Size = a.Content.LongLength,
                    CreatedUtc = a.CreatedUtc
                })
                .ToList();
        }

        public void Delete(int attachmentId)
        {
            var a = _context.Attachments.Find(attachmentId);
            if (a != null) _context.Attachments.Remove(a);
        }
    }
}
