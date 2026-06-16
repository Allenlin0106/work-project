using System.Configuration;
using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Web.Models;

namespace ProjectPlanning.Web.Controllers
{
    public class AuditLogsController : Controller
    {
        private readonly IAuditService _audit;
        private readonly ICurrentUserProvider _currentUser;

        public AuditLogsController(IAuditService audit, ICurrentUserProvider currentUser)
        {
            _audit = audit;
            _currentUser = currentUser;
        }

        public ActionResult Index(AuditLogFilterVM filter)
        {
            var role = ConfigurationManager.AppSettings["AuditAdminRole"];
            if (!string.IsNullOrEmpty(role) && !_currentUser.IsInRole(role))
            {
                return new HttpStatusCodeResult(403);
            }

            if (filter == null) filter = new AuditLogFilterVM();
            if (filter.Page < 1) filter.Page = 1;
            if (filter.PageSize < 1) filter.PageSize = 50;

            filter.Result = _audit.Search(new AuditQuery
            {
                WindowsAccount = filter.WindowsAccount,
                FromUtc = filter.FromUtc,
                ToUtc = filter.ToUtc,
                Action = filter.Action,
                EntityType = filter.EntityType,
                EntityKey = filter.EntityKey
            }, filter.Page, filter.PageSize);

            return View(filter);
        }
    }
}
