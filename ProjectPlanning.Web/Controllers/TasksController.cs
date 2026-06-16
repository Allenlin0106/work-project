using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        public ActionResult Create(int projectId)
        {
            return View("Edit", new TaskEditDto { ProjectId = projectId, StartDate = System.DateTime.UtcNow.Date, EndDate = System.DateTime.UtcNow.Date.AddDays(1) });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(TaskEditDto dto)
        {
            if (!ModelState.IsValid) return View("Edit", dto);
            _service.CreateTask(dto);
            return RedirectToAction("Details", "Projects", new { id = dto.ProjectId });
        }

        public ActionResult Edit(int id, int projectId)
        {
            var dto = new TaskEditDto { Id = id, ProjectId = projectId };
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TaskEditDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            _service.UpdateTask(dto);
            return RedirectToAction("Details", "Projects", new { id = dto.ProjectId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int projectId)
        {
            _service.DeleteTask(id);
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SetDependency(int predecessorId, int successorId, int projectId, DependencyType type = DependencyType.FinishToStart)
        {
            _service.SetDependency(predecessorId, successorId, type);
            return RedirectToAction("Index", "Gantt", new { projectId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RemoveDependency(int dependencyId, int projectId)
        {
            _service.RemoveDependency(dependencyId);
            return RedirectToAction("Index", "Gantt", new { projectId });
        }
    }
}
