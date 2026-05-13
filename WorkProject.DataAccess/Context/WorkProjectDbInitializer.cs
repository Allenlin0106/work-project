using System.Data.Entity;
using WorkProject.Contracts.Services;

namespace WorkProject.DataAccess.Context
{
    public class WorkProjectDbInitializer : IDatabaseInitializer
    {
        private readonly IPasswordHasher _hasher;

        public WorkProjectDbInitializer(IPasswordHasher hasher)
        {
            _hasher = hasher;
        }

        public void Initialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<WorkProjectDbContext>());

            using (var ctx = new WorkProjectDbContext())
            {
                ctx.Database.Initialize(force: false);
                DbSeeder.Seed(ctx, _hasher);
            }
        }
    }
}
