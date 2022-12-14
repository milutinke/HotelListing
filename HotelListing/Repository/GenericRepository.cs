using System.Linq.Expressions;
using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext? _context;
        private readonly DbSet<T>? _db;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db!.FindAsync(id);

            if (entity != null)
                _db!.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db!.RemoveRange(entities);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string>? includes = null)
        {
            IQueryable<T> query = _db!;

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                    query = query.Include(includeProperty);
            }

#pragma warning disable CS8603 // Possible null reference return.
            return await query!.AsNoTracking().FirstOrDefaultAsync(expression);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, List<string>? includes = null)
        {
            IQueryable<T> query = _db!;

            if (expression != null)
                query = query.Where(expression);

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                    query = query.Include(includeProperty);
            }

            if (orderBy != null)
                query = orderBy(query);

            return await query!.AsNoTracking().ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _db!.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _db!.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _db!.Attach(entity);
            _context!.Entry(entity).State = EntityState.Modified;
        }
    }
}
