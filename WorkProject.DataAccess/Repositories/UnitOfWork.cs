using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;

namespace WorkProject.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WorkProjectDbContext _context;

        public UnitOfWork(WorkProjectDbContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void SetCurrentUser(int? userId)
        {
            _context.SetCurrentUser(userId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
