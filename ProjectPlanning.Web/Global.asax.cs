using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL;
using ProjectPlanning.DAL.Migrations;
using ProjectPlanning.Web.App_Start;
using Rotativa;

namespace ProjectPlanning.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectPlanningDbContext, Configuration>());

            AutofacConfig.Register();
            RotativaConfiguration.Setup();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            var account = Session["Audited.WindowsAccount"] as string;
            if (string.IsNullOrEmpty(account)) return;

            try
            {
                var resolver = DependencyResolver.Current;
                var audit = resolver.GetService(typeof(IAuditService)) as IAuditService;
                audit?.LogLogout(account);
            }
            catch
            {
                // swallow - session end runs without HttpContext
            }
        }
    }
}
