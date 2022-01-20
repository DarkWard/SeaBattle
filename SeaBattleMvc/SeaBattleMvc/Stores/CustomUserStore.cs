using Microsoft.AspNetCore.Identity;
using SeaBattleORM;

namespace SeaBattleMvc
{
    public class CustomUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser>
    {
        private readonly UnitOfWork<AppUser> _unit;
        private readonly IRoleStore<AppRole> roleStore;
        private bool _disposed;

        public CustomUserStore(UnitOfWork<AppUser> unit)
        {
            _unit = unit;
        }

        public async Task<IdentityResult> CreateAsync(AppUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _unit.Repository.Create(user);

            var result = user;

            if (result != null)
            {
                return await Task.FromResult(IdentityResult.Success);
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not create user {user.Email}." });

        }
        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            _unit.Repository.Delete(user);

            return await Task.FromResult(IdentityResult.Success);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (userId == null) throw new ArgumentNullException(nameof(userId));

            if (!Int32.TryParse(userId, out int idInt))
            {
                throw new ArgumentException("Not a valid id", nameof(userId));
            }

            var result = _unit.Repository.Get(idInt);

            if (result == null)
            {
                throw new ArgumentException("This is not found", nameof(userId));
            }

            return Task.FromResult(result);
        }

        public Task<AppUser> FindByNameAsync(string normalizedUserName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (normalizedUserName == null)
            {
                throw new ArgumentNullException(nameof(normalizedUserName));
            }

            var users = _unit.Repository.GetAll();

            if (users.Any())
            {
                var user = users.ToList().FindAll(name => name.NormalizedUserName == normalizedUserName).FirstOrDefault();
                return Task.FromResult(user);
            }
            else
            {
                return Task.FromResult<AppUser>(null);
            }
        }

        public Task<string> GetPasswordHashAsync(AppUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = await roleStore.FindByIdAsync(user.RoleId.ToString(), cancellationToken);

            var roleName = await roleStore.GetRoleNameAsync(role, cancellationToken);

            return new List<string> { roleName };
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public async Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            var isHasPassword = String.IsNullOrWhiteSpace(_unit.Repository.Get(user.Id).UserPassword);

            return await Task.FromResult(isHasPassword);
        }

        public async Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = await roleStore.FindByNameAsync(roleName, cancellationToken);

            return role.Id == user.RoleId;
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }


        public Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}