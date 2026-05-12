using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Web.Filters;

namespace ProjectPlanning.Web.Controllers
{
    public class GanttController : Controller
    {
        private readonly IGanttService _gantt;

        public GanttController(IGanttService gantt)
        {
            _gantt = gantt;
        }

        [AuditAction("View", "Gantt", KeyRouteValue = "projectId")]
        public ActionResult Index(int projectId)
        {
            var data = _gantt.BuildGanttData(projectId);
            if (data == null) return HttpNotFound();
            return View(data);
        }

        public ActionResult GetData(int projectId)
        {
            var data = _gantt.BuildGanttData(projectId);
            if (data == null) return HttpNotFound();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
