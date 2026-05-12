using System.Linq;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        T GetById(params object[] keys);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
