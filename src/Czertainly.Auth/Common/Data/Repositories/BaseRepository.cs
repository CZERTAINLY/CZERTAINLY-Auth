using Czertainly.Auth.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await RepositoryContext.Set<TEntity>().Where(entity => entity.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await RepositoryContext.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
        }

        public void Create(TEntity entity)
        {
            this.RepositoryContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            this.RepositoryContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            this.RepositoryContext.Set<TEntity>().Remove(entity);
        }
    }
}
