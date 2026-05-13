using System;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Repositories;
using WorkProject.Contracts.Services;
using WorkProject.Domain.Entities;

namespace WorkProject.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly Func<IUserRepository> _userRepoFactory;
        private readonly Func<IUnitOfWork> _uowFactory;
        private readonly Func<IAuditService> _auditFactory;
        private readonly IPasswordHasher _hasher;
        private UserPrincipal _current;

        public AuthService(
            Func<IUserRepository> userRepoFactory,
            Func<IUnitOfWork> uowFactory,
            Func<IAuditService> auditFactory,
            IPasswordHasher hasher)
        {
            _userRepoFactory = userRepoFactory;
            _uowFactory = uowFactory;
            _auditFactory = auditFactory;
            _hasher = hasher;
            _current = new UserPrincipal { IsAuthenticated = false };
        }

        public UserPrincipal CurrentUser => _current;

        public AuthResult Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
            {
                return new AuthResult { Success = false, ErrorMessage = "User name and password are required." };
            }

            var repo = _userRepoFactory();
            var user = repo.GetByUserName(userName);
            if (user == null || !user.IsActive)
            {
                _auditFactory().RecordLogin(null, userName, success: false);
                return new AuthResult { Success = false, ErrorMessage = "Invalid credentials." };
            }

            var ok = _hasher.Verify(password, user.PasswordHash, user.PasswordSalt, user.PasswordIterations);
            if (!ok)
            {
                _auditFactory().RecordLogin(user.UserId, userName, success: false);
                return new AuthResult { Success = false, ErrorMessage = "Invalid credentials." };
            }

            _current = new UserPrincipal
            {
                UserId = user.UserId,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                IsAuthenticated = true
            };
            _auditFactory().RecordLogin(user.UserId, userName, success: true);
            return new AuthResult { Success = true, User = _current };
        }

        public void Logout()
        {
            if (_current?.IsAuthenticated == true)
            {
                _auditFactory().RecordLogout(_current.UserId, _current.UserName);
            }
            _current = new UserPrincipal { IsAuthenticated = false };
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
                throw new InvalidOperationException("New password must be at least 6 characters.");

            using (var uow = _uowFactory())
            {
                var repo = _userRepoFactory();
                var user = repo.GetById(userId);
                if (user == null) throw new InvalidOperationException("User not found.");
                if (!_hasher.Verify(oldPassword, user.PasswordHash, user.PasswordSalt, user.PasswordIterations))
                    throw new InvalidOperationException("Old password is incorrect.");

                var hashed = _hasher.Hash(newPassword);
                user.PasswordHash = hashed.Hash;
                user.PasswordSalt = hashed.Salt;
                user.PasswordIterations = hashed.Iterations;
                repo.Update(user);

                uow.SetCurrentUser(_current?.UserId);
                uow.SaveChanges();
            }
        }

        public int CreateUser(string userName, string displayName, string initialPassword)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new InvalidOperationException("User name is required.");
            if (string.IsNullOrWhiteSpace(displayName)) throw new InvalidOperationException("Display name is required.");
            if (string.IsNullOrEmpty(initialPassword) || initialPassword.Length < 6)
                throw new InvalidOperationException("Initial password must be at least 6 characters.");

            using (var uow = _uowFactory())
            {
                var repo = _userRepoFactory();
                if (repo.GetByUserName(userName) != null)
                    throw new InvalidOperationException("User name already exists.");

                var hashed = _hasher.Hash(initialPassword);
                var user = new User
                {
                    UserName = userName,
                    DisplayName = displayName,
                    PasswordHash = hashed.Hash,
                    PasswordSalt = hashed.Salt,
                    PasswordIterations = hashed.Iterations,
                    IsActive = true,
                    CreatedUtc = DateTime.UtcNow
                };
                repo.Add(user);

                uow.SetCurrentUser(_current?.UserId);
                uow.SaveChanges();
                return user.UserId;
            }
        }
    }

    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IAuthService _authService;

        public CurrentUserAccessor(IAuthService authService)
        {
            _authService = authService;
        }

        public int? CurrentUserId =>
            _authService.CurrentUser?.IsAuthenticated == true ? _authService.CurrentUser.UserId : (int?)null;

        public string CurrentUserName =>
            _authService.CurrentUser?.IsAuthenticated == true ? _authService.CurrentUser.UserName : null;
    }
}
