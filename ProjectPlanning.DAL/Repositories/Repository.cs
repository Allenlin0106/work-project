using System.Data.Entity;
using System.Linq;
using ProjectPlanning.DAL.Interfaces;

namespace ProjectPlanning.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ProjectPlanningDbContext Context;
        protected readonly DbSet<T> Set;

        public Repository(ProjectPlanningDbContext context)
        {
            Context = context;
            Set = context.Set<T>();
        }

        public IQueryable<T> Query() => Set.AsQueryable();

        public T GetById(params object[] keys) => Set.Find(keys);

        public void Add(T entity) => Set.Add(entity);

        public void Update(T entity)
        {
            Set.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity) => Set.Remove(entity);
    }
}
