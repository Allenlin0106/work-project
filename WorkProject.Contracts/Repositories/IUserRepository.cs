using System.Collections.Generic;
using WorkProject.Domain.Entities;

namespace WorkProject.Contracts.Repositories
{
    public interface IUserRepository
    {
        User GetByUserName(string userName);
        User GetById(int userId);
        void Add(User user);
        void Update(User user);
        IReadOnlyList<User> GetAll();
    }
}
