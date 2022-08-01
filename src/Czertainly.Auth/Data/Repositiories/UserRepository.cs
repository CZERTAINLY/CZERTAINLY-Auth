using Czertainly.Auth.Common.Data.Repositories;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Czertainly.Auth.Data.Repositiories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthDbContext repositoryContext) : base(repositoryContext, null, new[] { "Roles" })
        {
        }

    }
}
