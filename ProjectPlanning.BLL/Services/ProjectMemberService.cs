using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _members;
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;

        public ProjectMemberService(IProjectMemberRepository members, IUserRepository users, IUnitOfWork uow)
        {
            _members = members;
            _users = users;
            _uow = uow;
        }

        public IEnumerable<MemberDto> GetByProject(int projectId)
        {
            return _members.GetByProject(projectId).Select(m => new MemberDto
            {
                Id = m.Id,
                ProjectId = m.ProjectId,
                UserId = m.UserId,
                WindowsAccount = m.User != null ? m.User.WindowsAccount : null,
                DisplayName = m.User != null ? m.User.DisplayName : null,
                Role = m.Role,
                AddedUtc = m.AddedUtc
            }).ToList();
        }

        public int AddMember(int projectId, string windowsAccount, ProjectRole role)
        {
            var user = _users.GetByWindowsAccount(windowsAccount);
            if (user == null)
            {
                user = new User
                {
                    WindowsAccount = windowsAccount,
                    DisplayName = windowsAccount,
                    IsActive = true,
                    CreatedUtc = DateTime.UtcNow
                };
                _users.Add(user);
                _uow.SaveChanges();
            }

            var existing = _members.Find(projectId, user.Id);
            if (existing != null) return existing.Id;

            var member = new ProjectMember
            {
                ProjectId = projectId,
                UserId = user.Id,
                Role = role,
                AddedUtc = DateTime.UtcNow
            };
            _members.Add(member);
            _uow.SaveChanges();
            return member.Id;
        }

        public void RemoveMember(int memberId)
        {
            var existing = _members.GetById(memberId);
            if (existing == null) return;
            _members.Remove(existing);
            _uow.SaveChanges();
        }
    }
}
