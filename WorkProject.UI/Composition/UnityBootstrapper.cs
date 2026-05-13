using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;
using WorkProject.Business.Services;
using WorkProject.Contracts.Repositories;
using WorkProject.Contracts.Services;
using WorkProject.DataAccess.Context;
using WorkProject.DataAccess.Repositories;
using WorkProject.Infrastructure.Export;
using WorkProject.Infrastructure.Security;
using WorkProject.UI.ViewModels;
using WorkProject.UI.Views;

namespace WorkProject.UI.Composition
{
    public static class UnityBootstrapper
    {
        public static IUnityContainer Configure()
        {
            var container = new UnityContainer();

            container.RegisterType<IPasswordHasher, Pbkdf2PasswordHasher>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICurrentUserAccessor, CurrentUserAccessor>(new ContainerControlledLifetimeManager());

            container.RegisterType<WorkProjectDbContext>(new PerResolveLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<ICurrentUserAccessor>()));
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());

            container.RegisterType<IUserRepository, UserRepository>(new TransientLifetimeManager());
            container.RegisterType<IProjectRepository, ProjectRepository>(new TransientLifetimeManager());
            container.RegisterType<ITaskRepository, TaskRepository>(new TransientLifetimeManager());
            container.RegisterType<ICommentRepository, CommentRepository>(new TransientLifetimeManager());
            container.RegisterType<IAttachmentRepository, AttachmentRepository>(new TransientLifetimeManager());
            container.RegisterType<IAuditRepository, AuditRepository>(new TransientLifetimeManager());

            container.RegisterType<IProjectService, ProjectService>(new TransientLifetimeManager());
            container.RegisterType<ITaskService, TaskService>(new TransientLifetimeManager());
            container.RegisterType<IAuditService, AuditService>(new TransientLifetimeManager());
            container.RegisterType<IExportService, PdfExportService>(new TransientLifetimeManager());

            container.RegisterType<IDatabaseInitializer, WorkProjectDbInitializer>(new TransientLifetimeManager());

            RegisterFactory<IUserRepository>(container);
            RegisterFactory<IProjectRepository>(container);
            RegisterFactory<ITaskRepository>(container);
            RegisterFactory<ICommentRepository>(container);
            RegisterFactory<IAttachmentRepository>(container);
            RegisterFactory<IAuditRepository>(container);
            RegisterFactory<IUnitOfWork>(container);
            RegisterFactory<IAuditService>(container);

            container.RegisterType<LoginWindow>(new TransientLifetimeManager());
            container.RegisterType<MainWindow>(new TransientLifetimeManager());

            container.RegisterType<LoginViewModel>(new TransientLifetimeManager());
            container.RegisterType<MainViewModel>(new TransientLifetimeManager());
            container.RegisterType<ProjectListViewModel>(new TransientLifetimeManager());
            container.RegisterType<ProjectDetailViewModel>(new TransientLifetimeManager());
            container.RegisterType<ProjectGanttViewModel>(new TransientLifetimeManager());
            container.RegisterType<AuditLogViewModel>(new TransientLifetimeManager());
            container.RegisterType<TaskEditViewModel>(new TransientLifetimeManager());

            return container;
        }

        private static void RegisterFactory<T>(IUnityContainer container)
        {
            container.RegisterFactory<Func<T>>(c => new Func<T>(() => c.Resolve<T>()),
                new ContainerControlledLifetimeManager());
        }
    }
}
