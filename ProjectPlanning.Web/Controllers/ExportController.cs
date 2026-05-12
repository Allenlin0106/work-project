using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Web.Filters;
using Rotativa;
using Rotativa.Options;

namespace ProjectPlanning.Web.Controllers
{
    public class ExportController : Controller
    {
        private readonly IGanttService _gantt;

        public ExportController(IGanttService gantt)
        {
            _gantt = gantt;
        }

        [AuditAction("Export", "Project", Format = "Pdf")]
        public ActionResult ProjectPdf(int id)
        {
            var data = _gantt.BuildGanttData(id);
            if (data == null) return HttpNotFound();

            return new ActionAsPdf("PdfView", new { id })
            {
                FileName = string.Format("Project-{0}.pdf", id),
                PageSize = Size.A3,
                PageOrientation = Orientation.Landscape,
                CustomSwitches = "--javascript-delay 1500 --enable-local-file-access --print-media-type"
            };
        }

        public ActionResult PdfView(int id)
        {
            var data = _gantt.BuildGanttData(id);
            if (data == null) return HttpNotFound();
            return View(data);
        }
    }
}
