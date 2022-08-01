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
        private IResourceRepository _resource;
        private IActionRepository _action;

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

        public IResourceRepository Resource
        {
            get
            {
                if (_resource == null)
                {
                    _resource = new ResourceRepository(_dbContext);
                }
                return _resource;
            }
        }

        public IActionRepository Action
        {
            get
            {
                if (_action == null)
                {
                    _action = new ActionRepository(_dbContext);
                }
                return _action;
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
