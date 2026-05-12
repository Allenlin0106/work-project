using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectPlanning.BLL.Services;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Enums;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.Tests.Services
{
    [TestClass]
    public class GanttServiceTests
    {
        [TestMethod]
        public void BuildGanttData_ReturnsTasksAndDependencies()
        {
            var start = new DateTime(2026, 1, 1);
            var project = new Project { Id = 1, Name = "Demo", StartDate = start, EndDate = start.AddDays(10) };

            var t1 = new TaskItem { Id = 10, ProjectId = 1, Name = "T1", StartDate = start, EndDate = start.AddDays(2), ProgressPercent = 50, SortOrder = 1 };
            var t2 = new TaskItem { Id = 11, ProjectId = 1, Name = "T2", StartDate = start.AddDays(2), EndDate = start.AddDays(5), ProgressPercent = 0, SortOrder = 2 };

            var projects = new Mock<IProjectRepository>();
            projects.Setup(p => p.GetById(1)).Returns(project);
            var tasksRepo = new Mock<ITaskRepository>();
            tasksRepo.Setup(t => t.GetByProject(1)).Returns(new[] { t1, t2 });
            tasksRepo.Setup(t => t.GetDependenciesForProject(1)).Returns(new[]
            {
                new TaskDependency { Id = 1, PredecessorTaskId = 10, SuccessorTaskId = 11, Type = DependencyType.FinishToStart }
            });

            var sut = new GanttService(projects.Object, tasksRepo.Object);
            var data = sut.BuildGanttData(1);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.ProjectId);
            Assert.AreEqual(2, data.Tasks.Count);
            Assert.AreEqual(1, data.Dependencies.Count);
            Assert.AreEqual(10, data.Dependencies.First().PredecessorTaskId);
        }
    }
}
