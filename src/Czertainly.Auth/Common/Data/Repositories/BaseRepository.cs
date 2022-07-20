using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Czertainly.Auth.Common.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected DbContext RepositoryContext { get; set; }
        public BaseRepository(DbContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<TEntity> FindAll()
        {
            return RepositoryContext.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return RepositoryContext.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public async Task<PagedList<TEntity>> GetAllAsync(QueryStringParameters parameters)
        {
            return await PagedList<TEntity>.CreateAsync(FindAll(), parameters.PageNumber, parameters.ItemsPerPage);
        }

        public async Task<PagedList<TEntity>> GetWhereAsync(QueryStringParameters parameters, Expression<Func<TEntity, bool>> expression)
        {
            return await PagedList<TEntity>.CreateAsync(FindByCondition(expression), parameters.PageNumber, parameters.ItemsPerPage);
        }

        public async Task<TEntity> GetByIdAsync(IEntityKey entityKey)
        {
            return await GetTrackedEntityByKey(entityKey, "find");
        }

        public async Task<IEnumerable<TEntity>> GetByUuidsAsync(IEnumerable<Guid> uuids)
        {
            return await RepositoryContext.Set<TEntity>().Where(e => uuids.Contains(e.Uuid)).ToListAsync();
        }

        public async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await RepositoryContext.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
        }

        public void Create(TEntity entity)
        {
            RepositoryContext.Set<TEntity>().Add(entity);
        }

        public async Task UpdateAsync(IEntityKey entityKey, TEntity entity)
        {
            var trackedEntity = await GetTrackedEntityByKey(entityKey, "update");
            entity.Id = trackedEntity.Id;
            entity.Uuid = trackedEntity.Uuid;

            RepositoryContext.Entry(trackedEntity).CurrentValues.SetValues(entity);
        }

        public async Task DeleteAsync(IEntityKey entityKey)
        {
            var trackedEntity = await GetTrackedEntityByKey(entityKey, "delete");
            RepositoryContext.Set<TEntity>().Remove(trackedEntity);
        }

        public void Delete(TEntity entity)
        {
            RepositoryContext.Set<TEntity>().Remove(entity);
        }

        private async Task<TEntity> GetTrackedEntityByKey(IEntityKey entityKey, string operation)
        {
            if (entityKey.Uuid == null && entityKey.Id == null) throw new EntityNotFoundException($"Cannot {operation} entity {typeof(TEntity).Name} with invalid id");

            var context = RepositoryContext.Set<TEntity>();
            var trackedEntity = entityKey.Uuid.HasValue ? await context.FirstOrDefaultAsync(e => e.Uuid.Equals(entityKey.Uuid)) : await context.FindAsync(entityKey.Id.Value);
            if (trackedEntity == null) throw new EntityNotFoundException($"Cannot {operation} entity {typeof(TEntity).Name} with id {(entityKey.Uuid.HasValue ? entityKey.Uuid : entityKey.Id.Value)}");

            return trackedEntity;
        }
    }
}
