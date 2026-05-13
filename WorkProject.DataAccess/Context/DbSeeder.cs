using System;
using System.Linq;
using WorkProject.Contracts.Services;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Context
{
    internal static class DbSeeder
    {
        public const string DefaultAdminUserName = "admin";
        public const string DefaultAdminPassword = "Admin#12345";

        public static void Seed(WorkProjectDbContext context, IPasswordHasher hasher)
        {
            if (context.Users.Any(u => u.UserName == DefaultAdminUserName)) return;

            var hashed = hasher.Hash(DefaultAdminPassword);
            var admin = new User
            {
                UserName = DefaultAdminUserName,
                DisplayName = "System Administrator",
                PasswordHash = hashed.Hash,
                PasswordSalt = hashed.Salt,
                PasswordIterations = hashed.Iterations,
                IsActive = true,
                CreatedUtc = DateTime.UtcNow
            };
            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
