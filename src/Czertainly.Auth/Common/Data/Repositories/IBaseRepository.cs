using System.Linq.Expressions;
using Czertainly.Auth.Common.Data;
using Czertainly.Auth.Common.Models.Entities;

namespace Czertainly.Auth.Common.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity, new()
    {
        IQueryable<TEntity> FindAll();
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);

        Task<PagedList<TEntity>> GetAllAsync(QueryStringParameters parameters);
        Task<PagedList<TEntity>> GetWhereAsync(QueryStringParameters parameters, Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> expression);

        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
