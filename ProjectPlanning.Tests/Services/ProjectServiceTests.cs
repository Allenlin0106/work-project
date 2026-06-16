using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.BLL.Services;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.Tests.Services
{
    [TestClass]
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository> _projects;
        private Mock<IProjectMemberRepository> _members;
        private Mock<IUnitOfWork> _uow;
        private Mock<ICurrentUserProvider> _currentUser;
        private ProjectService _sut;

        [TestInitialize]
        public void Setup()
        {
            _projects = new Mock<IProjectRepository>();
            _members = new Mock<IProjectMemberRepository>();
            _uow = new Mock<IUnitOfWork>();
            _currentUser = new Mock<ICurrentUserProvider>();
            _currentUser.Setup(c => c.GetOrProvision()).Returns(new User { Id = 1, WindowsAccount = "DOMAIN\\me" });
            _sut = new ProjectService(_projects.Object, _members.Object, _uow.Object, _currentUser.Object);
        }

        [TestMethod]
        public void Create_AssignsOwner_AndAddsOwnerMembership()
        {
            var dto = new ProjectEditDto
            {
                Name = "P1",
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(7),
                Status = ProjectStatus.Active
            };
            Project added = null;
            _projects.Setup(p => p.Add(It.IsAny<Project>())).Callback<Project>(p => { p.Id = 42; added = p; });

            var id = _sut.Create(dto);

            Assert.AreEqual(42, id);
            Assert.IsNotNull(added);
            Assert.AreEqual(1, added.OwnerId);
            _members.Verify(m => m.Add(It.Is<ProjectMember>(pm => pm.ProjectId == 42 && pm.UserId == 1 && pm.Role == ProjectRole.Owner)), Times.Once);
            _uow.Verify(u => u.SaveChanges(), Times.AtLeast(2));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_RejectsEndBeforeStart()
        {
            _sut.Create(new ProjectEditDto
            {
                Name = "Bad",
                StartDate = DateTime.UtcNow.Date.AddDays(5),
                EndDate = DateTime.UtcNow.Date
            });
        }

        [TestMethod]
        public void ListForCurrentUser_ProjectsThroughOwnerOrMembership()
        {
            var data = new List<Project>
            {
                new Project { Id = 1, Name = "A", OwnerId = 1, Owner = new User { DisplayName = "me" }, Tasks = new List<TaskItem>(), Members = new List<ProjectMember>() }
            }.AsQueryable();
            _projects.Setup(p => p.ListForUser(1)).Returns(data);

            var list = _sut.ListForCurrentUser().ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("A", list[0].Name);
        }
    }
}
