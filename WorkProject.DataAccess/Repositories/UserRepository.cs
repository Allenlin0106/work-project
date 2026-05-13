using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WorkProjectDbContext _context;

        public UserRepository(WorkProjectDbContext context)
        {
            _context = context;
        }

        public User GetByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public User GetById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public IReadOnlyList<User> GetAll()
        {
            return _context.Users.OrderBy(u => u.UserName).ToList();
        }
    }
}
