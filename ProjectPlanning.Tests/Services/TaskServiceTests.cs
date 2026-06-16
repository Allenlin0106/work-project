using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectPlanning.BLL.Services;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.Tests.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        [TestMethod]
        public void CreateTask_PersistsEntity()
        {
            var tasks = new Mock<ITaskRepository>();
            TaskItem added = null;
            tasks.Setup(t => t.Add(It.IsAny<TaskItem>())).Callback<TaskItem>(t => { t.Id = 7; added = t; });
            var uow = new Mock<IUnitOfWork>();

            var sut = new TaskService(tasks.Object, uow.Object);
            var id = sut.CreateTask(new TaskEditDto
            {
                ProjectId = 1,
                Name = "X",
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(1),
                ProgressPercent = 10,
                SortOrder = 5
            });

            Assert.AreEqual(7, id);
            Assert.IsNotNull(added);
            Assert.AreEqual("X", added.Name);
            uow.Verify(u => u.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDependency_RejectsSelfReference()
        {
            var sut = new TaskService(new Mock<ITaskRepository>().Object, new Mock<IUnitOfWork>().Object);
            sut.SetDependency(5, 5, DependencyType.FinishToStart);
        }
    }
}
