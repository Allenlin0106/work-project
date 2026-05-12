using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.BLL.Services;
using ProjectPlanning.DAL;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.DAL.Repositories;
using ProjectPlanning.Web.Infrastructure;

namespace ProjectPlanning.Web.App_Start
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();

            builder.RegisterType<HttpContextCurrentUserAccessor>()
                .As<ICurrentUserAccessor>()
                .InstancePerRequest();

            builder.RegisterType<ProjectPlanningDbContext>()
                .AsSelf()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(ProjectRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(ProjectService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<HttpContextCurrentUserProvider>()
                .As<ICurrentUserProvider>()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
