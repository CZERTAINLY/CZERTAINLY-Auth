using Czertainly.Auth.Common.Exceptions;
using Czertainly.Auth.Common.Extensions;
using Czertainly.Auth.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Czertainly.Auth.Common.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected DbContext _dbContext;
        protected DbSet<TEntity> _dbSet;

        protected readonly string[]? _includes = null;
        protected readonly string[]? _includesDetail = null;

        public BaseRepository(DbContext repositoryContext)
        {
            _dbContext = repositoryContext;
            _dbSet = repositoryContext.Set<TEntity>();
        }

        public BaseRepository(DbContext repositoryContext, string[]? includes, string[]? includesDetail)
            : this(repositoryContext)
        {
            _includes = includes;
            _includesDetail = includesDetail;
        }

        #region Get operations for read-only purpose (No Tracking)

        public IQueryable<TEntity> FindAll()
        {
            return QueryWithCrudIncludes(false).AsNoTracking();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return QueryWithCrudIncludes(false).Where(expression).AsNoTracking();
        }

        public async Task<PagedList<TEntity>> GetAllAsync(QueryStringParameters parameters)
        {
            return await PagedList<TEntity>.CreateAsync(FindAll().OrderBy(parameters.SortBy, parameters.SortAscending), parameters.PageNumber, parameters.ItemsPerPage);
        }

        public async Task<PagedList<TEntity>> GetWhereAsync(QueryStringParameters parameters, Expression<Func<TEntity, bool>> expression)
        {
            return await PagedList<TEntity>.CreateAsync(FindByCondition(expression).OrderBy(parameters.SortBy, parameters.SortAscending), parameters.PageNumber, parameters.ItemsPerPage);
        }

        #endregion

        public async Task<TEntity> GetByKeyAsync(IEntityKey entityKey)
        {
            return await GetTrackedEntityByKey(entityKey, "find");
        }

        public async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.Where(expression).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetByUuidsAsync(IEnumerable<Guid> uuids)
        {
            return await _dbSet.Where(e => uuids.Contains(e.Uuid)).ToListAsync();
        }

        public async Task<Dictionary<TKey, TEntity>> GetDictionaryMap<TKey>(Func<TEntity, TKey> keySelector, Expression<Func<TEntity, bool>>? expression = null) where TKey : notnull
        {
            var context = _dbSet;
            if (expression != null) context.Where(expression);
            return await context.ToDictionaryAsync(keySelector);
        }

        #region CRUD operations

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task UpdateAsync(IEntityKey entityKey, TEntity entity)
        {
            var trackedEntity = await GetTrackedEntityByKey(entityKey, "update");
            entity.Id = trackedEntity.Id;
            entity.Uuid = trackedEntity.Uuid;

            _dbContext.Entry(trackedEntity).CurrentValues.SetValues(entity);
        }

        public async Task DeleteAsync(IEntityKey entityKey)
        {
            var trackedEntity = await GetTrackedEntityByKey(entityKey, "delete");
            _dbSet.Remove(trackedEntity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Reload(TEntity entity)
        {
            _dbContext.Entry(entity).Reload();
        }

        #endregion

        protected IQueryable<TEntity> QueryWithCrudIncludes(bool detailResponse)
        {
            var includes = detailResponse ? _includesDetail : _includes;
            if (includes == null || includes.Length == 0) return _dbSet;

            IQueryable<TEntity> query = _dbSet;
            foreach (var include in includes) query = query.Include(include);

            return query;
        }

        #region Private methods

        private async Task<TEntity> GetTrackedEntityByKey(IEntityKey entityKey, string operation)
        {
            if (entityKey.Uuid == null && entityKey.Id == null) throw new EntityNotFoundException($"Cannot {operation} entity {typeof(TEntity).Name} with invalid id");

            var query = QueryWithCrudIncludes(true);
            var trackedEntity = entityKey.Uuid.HasValue ? await query.FirstOrDefaultAsync(e => e.Uuid.Equals(entityKey.Uuid)) : await query.FirstOrDefaultAsync(e => e.Id == entityKey.Id.Value);
            if (trackedEntity == null) throw new EntityNotFoundException($"Cannot {operation} entity {typeof(TEntity).Name} with id {(entityKey.Uuid.HasValue ? entityKey.Uuid : entityKey.Id.Value)}");

            return trackedEntity;
        }
        
        #endregion

    }
}
