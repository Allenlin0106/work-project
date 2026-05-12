using System.Web;
using ProjectPlanning.DAL.Interfaces;

namespace ProjectPlanning.Web.Infrastructure
{
    public class HttpContextCurrentUserAccessor : ICurrentUserAccessor
    {
        public string WindowsAccount
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null || ctx.User == null || ctx.User.Identity == null) return null;
                return ctx.User.Identity.Name;
            }
        }

        public int? UserId
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null) return null;
                var value = ctx.Items["CurrentUserId"];
                return value is int id ? id : (int?)null;
            }
        }

        public string IpAddress
        {
            get
            {
                var ctx = HttpContext.Current;
                return ctx != null && ctx.Request != null ? ctx.Request.UserHostAddress : null;
            }
        }

        public string UserAgent
        {
            get
            {
                var ctx = HttpContext.Current;
                return ctx != null && ctx.Request != null ? ctx.Request.UserAgent : null;
            }
        }

        public string ControllerAction
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null) return null;
                return ctx.Items["ControllerAction"] as string;
            }
        }
    }
}
