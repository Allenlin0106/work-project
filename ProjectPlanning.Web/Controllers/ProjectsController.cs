using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Web.Filters;

namespace ProjectPlanning.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View(_service.ListForCurrentUser());
        }

        [AuditAction("View", "Project")]
        public ActionResult Details(int id)
        {
            var detail = _service.Get(id);
            if (detail == null) return HttpNotFound();
            return View(detail);
        }

        public ActionResult Create()
        {
            return View(new ProjectEditDto());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ProjectEditDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var id = _service.Create(dto);
            return RedirectToAction("Details", new { id });
        }

        public ActionResult Edit(int id)
        {
            var detail = _service.Get(id);
            if (detail == null) return HttpNotFound();
            return View(new ProjectEditDto
            {
                Id = detail.Id,
                Name = detail.Name,
                Description = detail.Description,
                StartDate = detail.StartDate,
                EndDate = detail.EndDate,
                Status = detail.Status
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectEditDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            _service.Update(dto);
            return RedirectToAction("Details", new { id = dto.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
