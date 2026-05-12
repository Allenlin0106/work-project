using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly IProjectMemberService _members;

        public MembersController(IProjectMemberService members)
        {
            _members = members;
        }

        public ActionResult Index(int projectId)
        {
            ViewBag.ProjectId = projectId;
            return View(_members.GetByProject(projectId));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(int projectId, string windowsAccount, ProjectRole role)
        {
            if (!string.IsNullOrWhiteSpace(windowsAccount))
            {
                _members.AddMember(projectId, windowsAccount, role);
            }
            return RedirectToAction("Index", new { projectId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Remove(int memberId, int projectId)
        {
            _members.RemoveMember(memberId);
            return RedirectToAction("Index", new { projectId });
        }
    }
}
