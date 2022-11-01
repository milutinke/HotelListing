using System.Linq.Expressions;

namespace HotelListing.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync(
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
            List<string>? strings = null
        );

        Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string>? strings = null);

        Task InsertAsync(T entity);

        Task InsertRangeAsync(IEnumerable<T> entities);

        Task DeleteAsync(int id);

        void DeleteRange(IEnumerable<T> entities);

        void Update(T entity);
    }
}
