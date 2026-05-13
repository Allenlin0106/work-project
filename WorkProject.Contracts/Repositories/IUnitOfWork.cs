using System;

namespace WorkProject.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void SetCurrentUser(int? userId);
    }
}
