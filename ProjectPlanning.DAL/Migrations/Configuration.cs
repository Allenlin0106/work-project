using System;
using System.Data.Entity.Migrations;
using System.Linq;
using ProjectPlanning.Entities.Enums;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ProjectPlanningDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ProjectPlanning.DAL.ProjectPlanningDbContext";
        }

        protected override void Seed(ProjectPlanningDbContext db)
        {
            var owner = db.Users.FirstOrDefault(u => u.WindowsAccount == "SEED\\demo-owner");
            if (owner == null)
            {
                owner = new User
                {
                    WindowsAccount = "SEED\\demo-owner",
                    DisplayName = "Demo Owner",
                    Email = "demo.owner@example.com",
                    IsActive = true,
                    CreatedUtc = DateTime.UtcNow
                };
                db.Users.Add(owner);
                db.SaveChanges();
            }

            if (db.Projects.Any()) return;

            var start = DateTime.UtcNow.Date;
            var project = new Project
            {
                Name = "Demo Project",
                Description = "Sample project for first run.",
                StartDate = start,
                EndDate = start.AddDays(30),
                Status = ProjectStatus.Active,
                OwnerId = owner.Id,
                CreatedUtc = DateTime.UtcNow,
                ModifiedUtc = DateTime.UtcNow
            };
            db.Projects.Add(project);
            db.SaveChanges();

            db.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = project.Id,
                UserId = owner.Id,
                Role = ProjectRole.Owner,
                AddedUtc = DateTime.UtcNow
            });

            var t1 = new TaskItem { ProjectId = project.Id, Name = "Requirements", StartDate = start, EndDate = start.AddDays(5), ProgressPercent = 100, SortOrder = 1, CreatedUtc = DateTime.UtcNow, ModifiedUtc = DateTime.UtcNow };
            var t2 = new TaskItem { ProjectId = project.Id, Name = "Design", StartDate = start.AddDays(5), EndDate = start.AddDays(10), ProgressPercent = 60, SortOrder = 2, CreatedUtc = DateTime.UtcNow, ModifiedUtc = DateTime.UtcNow };
            var t3 = new TaskItem { ProjectId = project.Id, Name = "Implementation", StartDate = start.AddDays(10), EndDate = start.AddDays(22), ProgressPercent = 20, SortOrder = 3, CreatedUtc = DateTime.UtcNow, ModifiedUtc = DateTime.UtcNow };
            var t4 = new TaskItem { ProjectId = project.Id, Name = "Testing", StartDate = start.AddDays(22), EndDate = start.AddDays(28), ProgressPercent = 0, SortOrder = 4, CreatedUtc = DateTime.UtcNow, ModifiedUtc = DateTime.UtcNow };
            var t5 = new TaskItem { ProjectId = project.Id, Name = "Release", StartDate = start.AddDays(28), EndDate = start.AddDays(30), ProgressPercent = 0, SortOrder = 5, CreatedUtc = DateTime.UtcNow, ModifiedUtc = DateTime.UtcNow };
            db.Tasks.AddRange(new[] { t1, t2, t3, t4, t5 });
            db.SaveChanges();

            db.TaskDependencies.Add(new TaskDependency { PredecessorTaskId = t1.Id, SuccessorTaskId = t2.Id, Type = DependencyType.FinishToStart });
            db.TaskDependencies.Add(new TaskDependency { PredecessorTaskId = t2.Id, SuccessorTaskId = t3.Id, Type = DependencyType.FinishToStart });
            db.TaskDependencies.Add(new TaskDependency { PredecessorTaskId = t3.Id, SuccessorTaskId = t4.Id, Type = DependencyType.FinishToStart });
            db.TaskDependencies.Add(new TaskDependency { PredecessorTaskId = t4.Id, SuccessorTaskId = t5.Id, Type = DependencyType.FinishToStart });
            db.SaveChanges();
        }
    }
}
