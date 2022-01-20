using Microsoft.AspNetCore.Identity;
using SeaBattleORM;

namespace SeaBattleMvc
{
    public class CustomRoleStore : IRoleStore<AppRole>
    {
        private readonly UnitOfWork<AppRole> _unit;
        private bool _disposed;

        public CustomRoleStore(UnitOfWork<AppRole> unit)
        {
            _unit = unit;
        }

        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            _unit.Repository.Create(role);

            var result = role;

            if (result != null)
            {
                return await Task.FromResult(IdentityResult.Success);
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not create role{role}." });
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            _unit.Repository.Delete(role);

            return await Task.FromResult(IdentityResult.Success);
        }

        public Task<AppRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (roleId == null) throw new ArgumentNullException(nameof(roleId));

            if (!Int32.TryParse(roleId, out int idInt))
            {
                throw new ArgumentException("Not a valid id", nameof(roleId));
            }

            var result = _unit.Repository.Get(idInt);

            if (result == null)
            {
                throw new ArgumentException("This is not found", nameof(roleId));
            }

            return Task.FromResult(result);
        }

        public Task<AppRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (normalizedRoleName == null) throw new ArgumentNullException(nameof(normalizedRoleName));

            var roles = _unit.Repository.GetAll().ToList();

            var role = roles.FindAll(name => name.NormalizedRoleName == normalizedRoleName).FirstOrDefault();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            return Task.FromResult(role);
        }

        public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = _unit.Repository.Get(role.Id).Name;

            if (String.IsNullOrEmpty(result))
            {
                throw new ArgumentException("This user name not found", nameof(role));
            }

            return Task.FromResult(result);
        }

        public async Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            var userId = _unit.Repository.Get(role.Id).Id.ToString();

            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("This role not have password hash", nameof(role));
            }

            return await Task.FromResult(userId);
        }

        public async Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            var userName = _unit.Repository.Get(role.Id).RoleName;

            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("This user not have role name", nameof(role));
            }

            return await Task.FromResult(userName);
        }

        public async Task SetNormalizedRoleNameAsync(AppRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrWhiteSpace(normalizedName)) throw new ArgumentNullException(nameof(normalizedName));

            role.NormalizedName = normalizedName;

            await Task.FromResult<object>(null);
        }

        public async Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            await SetNormalizedRoleNameAsync(role, roleName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null) throw new ArgumentNullException(nameof(role));

            var getRole = _unit.Repository.Get(role.Id);

            if (getRole == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not update role {role}." });
            }

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
    }
}