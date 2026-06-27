using generic_repo_uow_pattern.Data;
using generic_repo_uow_pattern.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace generic_repo_uow_pattern.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        private ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection> FindAllAsync(Expression<Func<T, bool>> match)
            => await _dbSet.Where(match).ToListAsync();

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
            => await _dbSet.SingleOrDefaultAsync(match);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<(ICollection<T> Result, int TotalNumber, int TotalPages, bool IsPrevious, bool IsNext)> SearchOrderAndPaginateAsync(Expression<Func<T, bool>> searchPredicate = null, Expression<Func<T, object>> orderBy = null, bool isDescending = false, int? pageNumber = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            if(searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            if(orderBy != null)
            {
                if (isDescending)
                {
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    query = query.OrderBy(orderBy);
                }
            }

            foreach(var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            int totalNumber = await query.CountAsync();

            if(pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value-1) * pageSize.Value).Take(pageSize.Value);
            }

            int? totalPages = pageNumber.HasValue && pageSize.HasValue ? (int?)Math.Ceiling((double)totalNumber /  pageSize.Value) : null;
            bool? isPrevious = pageNumber.HasValue ? pageNumber > 1 : null;
            bool? isNext = pageNumber.HasValue && totalPages.HasValue ? pageNumber < totalPages : null;

            ICollection<T> result = await query.ToListAsync();

            return (result, totalNumber, totalPages ?? 0, isPrevious ?? false, isNext ?? false);
        }

        public void SetDbContext(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
