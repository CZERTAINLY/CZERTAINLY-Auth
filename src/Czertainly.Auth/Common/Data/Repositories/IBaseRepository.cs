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
        Task<TEntity> GetByIdAsync(IEntityKey entityKey);
        Task<IEnumerable<TEntity>> GetByUuidsAsync(IEnumerable<Guid> uuids);
        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> expression);

        void Create(TEntity entity);
        Task UpdateAsync(IEntityKey entityKey, TEntity entity);
        Task DeleteAsync(IEntityKey entityKey);
        void Delete(TEntity entity);
    }
}
