using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _comments;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserProvider _currentUser;

        public CommentService(ICommentRepository comments, IUnitOfWork uow, ICurrentUserProvider currentUser)
        {
            _comments = comments;
            _uow = uow;
            _currentUser = currentUser;
        }

        public IEnumerable<CommentDto> GetByProject(int projectId)
        {
            return _comments.GetByProject(projectId).Select(Map).ToList();
        }

        public IEnumerable<CommentDto> GetByTask(int taskId)
        {
            return _comments.GetByTask(taskId).Select(Map).ToList();
        }

        public int AddComment(int projectId, int? taskId, string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new InvalidOperationException("Comment body is required.");

            var user = _currentUser.GetOrProvision();
            var entity = new Comment
            {
                ProjectId = projectId,
                TaskId = taskId,
                AuthorId = user.Id,
                Body = body.Trim(),
                CreatedUtc = DateTime.UtcNow
            };
            _comments.Add(entity);
            _uow.SaveChanges();
            return entity.Id;
        }

        public void DeleteComment(int commentId)
        {
            var entity = _comments.GetById(commentId);
            if (entity == null) return;
            _comments.Remove(entity);
            _uow.SaveChanges();
        }

        private static CommentDto Map(Comment c)
        {
            return new CommentDto
            {
                Id = c.Id,
                ProjectId = c.ProjectId,
                TaskId = c.TaskId,
                AuthorId = c.AuthorId,
                AuthorDisplayName = c.Author != null ? c.Author.DisplayName : null,
                Body = c.Body,
                CreatedUtc = c.CreatedUtc
            };
        }
    }
}
