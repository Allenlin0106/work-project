using System;
using System.Collections.Generic;
using System.IO;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachments;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserProvider _currentUser;

        public AttachmentService(IAttachmentRepository attachments, IUnitOfWork uow, ICurrentUserProvider currentUser)
        {
            _attachments = attachments;
            _uow = uow;
            _currentUser = currentUser;
        }

        public IEnumerable<Attachment> GetByProject(int projectId)
        {
            return _attachments.GetByProject(projectId);
        }

        public int Save(int projectId, int? taskId, string fileName, string contentType, Stream content, string storageRoot)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new InvalidOperationException("File name is required.");
            if (content == null)
                throw new InvalidOperationException("Attachment content is required.");

            var user = _currentUser.GetOrProvision();
            var safeName = Path.GetFileName(fileName);
            var dir = Path.Combine(storageRoot, projectId.ToString());
            Directory.CreateDirectory(dir);
            var uniqueName = Guid.NewGuid().ToString("N") + "_" + safeName;
            var fullPath = Path.Combine(dir, uniqueName);

            using (var fs = File.Create(fullPath))
            {
                content.CopyTo(fs);
            }

            var info = new FileInfo(fullPath);
            var entity = new Attachment
            {
                ProjectId = projectId,
                TaskId = taskId,
                FileName = safeName,
                ContentType = contentType,
                SizeBytes = info.Length,
                StoragePath = Path.Combine(projectId.ToString(), uniqueName),
                UploadedById = user.Id,
                UploadedUtc = DateTime.UtcNow
            };
            _attachments.Add(entity);
            _uow.SaveChanges();
            return entity.Id;
        }

        public void Delete(int attachmentId, string storageRoot)
        {
            var entity = _attachments.GetById(attachmentId);
            if (entity == null) return;

            var fullPath = Path.Combine(storageRoot, entity.StoragePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            _attachments.Remove(entity);
            _uow.SaveChanges();
        }
    }
}
