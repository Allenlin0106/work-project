using System.Web.Mvc;

namespace ProjectPlanning.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Projects");
        }
    }
}
