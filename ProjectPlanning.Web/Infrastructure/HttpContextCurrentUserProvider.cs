using System;
using System.Security.Principal;
using System.Web;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.Web.Infrastructure
{
    public class HttpContextCurrentUserProvider : ICurrentUserProvider
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;

        public HttpContextCurrentUserProvider(IUserRepository users, IUnitOfWork uow)
        {
            _users = users;
            _uow = uow;
        }

        public string WindowsAccount
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null || ctx.User == null || ctx.User.Identity == null) return null;
                return ctx.User.Identity.Name;
            }
        }

        public User GetOrProvision()
        {
            var account = WindowsAccount;
            if (string.IsNullOrWhiteSpace(account))
                throw new InvalidOperationException("No authenticated Windows identity.");

            var user = _users.GetByWindowsAccount(account);
            if (user != null) return user;

            user = new User
            {
                WindowsAccount = account,
                DisplayName = ResolveDisplayName(account) ?? account,
                IsActive = true,
                CreatedUtc = DateTime.UtcNow
            };
            _users.Add(user);
            _uow.SaveChanges();
            return user;
        }

        public bool IsInRole(string role)
        {
            var ctx = HttpContext.Current;
            if (ctx == null || ctx.User == null) return false;
            return ctx.User.IsInRole(role);
        }

        private static string ResolveDisplayName(string account)
        {
            try
            {
                var idx = account.IndexOf('\\');
                var name = idx >= 0 ? account.Substring(idx + 1) : account;
                return name;
            }
            catch
            {
                return null;
            }
        }
    }
}
