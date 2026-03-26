using System.Linq.Expressions;

namespace TravelTrek.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void UpdateRange(IReadOnlyList<T> entities);
        void Delete(T entity);
        void DeleteRange(IReadOnlyList<T> entities);
    }
}
