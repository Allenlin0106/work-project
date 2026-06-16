using System;
using System.Web.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Common.Auditing;

namespace ProjectPlanning.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuditActionAttribute : ActionFilterAttribute
    {
        public string Action { get; }
        public string EntityType { get; }
        public string Format { get; set; }
        public string KeyRouteValue { get; set; } = "id";

        public AuditActionAttribute(string action, string entityType)
        {
            Action = action;
            EntityType = entityType;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null && !filterContext.ExceptionHandled) return;

            var resolver = DependencyResolver.Current;
            var audit = (IAuditService)resolver.GetService(typeof(IAuditService));
            if (audit == null) return;

            var key = filterContext.RouteData.Values.ContainsKey(KeyRouteValue)
                ? filterContext.RouteData.Values[KeyRouteValue]?.ToString()
                : null;

            if (string.Equals(Action, AuditAction.Export, StringComparison.OrdinalIgnoreCase))
            {
                audit.LogExport(EntityType, key, Format ?? "Pdf");
            }
            else if (string.Equals(Action, AuditAction.View, StringComparison.OrdinalIgnoreCase))
            {
                audit.LogView(EntityType, key);
            }
        }
    }
}
