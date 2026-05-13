using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly WorkProjectDbContext _context;

        public ProjectRepository(WorkProjectDbContext context)
        {
            _context = context;
        }

        public Project GetById(int projectId, bool includeMembers = false)
        {
            var query = _context.Projects.Include(p => p.Owner);
            if (includeMembers)
                query = query.Include(p => p.Members.Select(m => m.User));
            return query.FirstOrDefault(p => p.ProjectId == projectId);
        }

        public IReadOnlyList<Project> GetForUser(int userId)
        {
            return _context.Projects
                .Include(p => p.Owner)
                .Where(p => p.OwnerUserId == userId || p.Members.Any(m => m.UserId == userId))
                .OrderBy(p => p.Name)
                .ToList();
        }

        public IReadOnlyList<Project> GetAll()
        {
            return _context.Projects.Include(p => p.Owner).OrderBy(p => p.Name).ToList();
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }

        public void Update(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
        }

        public void Delete(int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null) _context.Projects.Remove(project);
        }

        public void AddMember(ProjectMember member)
        {
            _context.ProjectMembers.Add(member);
        }

        public void RemoveMember(int projectMemberId)
        {
            var member = _context.ProjectMembers.Find(projectMemberId);
            if (member != null) _context.ProjectMembers.Remove(member);
        }

        public ProjectMember GetMember(int projectMemberId)
        {
            return _context.ProjectMembers.Include(m => m.User).FirstOrDefault(m => m.ProjectMemberId == projectMemberId);
        }
    }
}
