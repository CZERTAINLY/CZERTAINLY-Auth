using Czertainly.Auth.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czertainly.Auth.Data.Repositiories
{
    public class RepositoryManager : IRepositoryManager
    {
        private AuthDbContext _dbContext;

        private IUserRepository _user;
        private IRoleRepository _role;
        private IPermissionRepository _permission;
        private IEndpointRepository _endpoint;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_dbContext);
                }
                return _user;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_dbContext);
                }
                return _role;
            }
        }

        public IPermissionRepository Permission
        {
            get
            {
                if (_permission == null)
                {
                    _permission = new PermissionRepository(_dbContext);
                }
                return _permission;
            }
        }

        public IEndpointRepository Endpoint
        {
            get
            {
                if (_endpoint == null)
                {
                    _endpoint = new EndpointRepository(_dbContext);
                }
                return _endpoint;
            }
        }

        public RepositoryManager(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
