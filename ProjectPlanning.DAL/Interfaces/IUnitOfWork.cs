using System;
using System.Data.Entity;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        DbContextTransaction BeginTransaction();
    }
}
