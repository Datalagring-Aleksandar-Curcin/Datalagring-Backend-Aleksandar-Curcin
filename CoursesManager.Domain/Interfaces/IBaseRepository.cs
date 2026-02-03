using System.Linq.Expressions;

namespace CoursesManager.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> findBy);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}