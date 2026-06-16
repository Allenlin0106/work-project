using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using ProjectPlanning.BLL.Interfaces;

namespace ProjectPlanning.Web.Filters
{
    public class AuthenticatedUserFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if (ctx.User == null || ctx.User.Identity == null || !ctx.User.Identity.IsAuthenticated) return;

            var resolver = DependencyResolver.Current;
            var currentUser = (ICurrentUserProvider)resolver.GetService(typeof(ICurrentUserProvider));
            var auditService = (IAuditService)resolver.GetService(typeof(IAuditService));

            var user = currentUser.GetOrProvision();
            ctx.Items["CurrentUserId"] = user.Id;

            var session = ctx.Session;
            if (session != null && session["Audited.Login"] == null)
            {
                session["Audited.Login"] = true;
                session["Audited.WindowsAccount"] = user.WindowsAccount;
                auditService.LogLogin(user.WindowsAccount);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            ctx.Items["ControllerAction"] = controller + "." + action;
        }
    }
}
