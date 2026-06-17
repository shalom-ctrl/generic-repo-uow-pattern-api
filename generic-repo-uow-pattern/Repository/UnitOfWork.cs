using generic_repo_uow_pattern.Data;
using generic_repo_uow_pattern.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace generic_repo_uow_pattern.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories;
        public IProductRepository ProductRepository { get; }
        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
            ProductRepository = new ProductRepository(_dbContext);
        }
        public async Task BeginTransactionAsync()

        {
            _dbTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbTransaction.CommitAsync();
            }
            catch
            {
                await _dbTransaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _dbTransaction.DisposeAsync();
                _dbTransaction = null!;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)_repositories[typeof(T)];
            }

            var repositoryInstance = new Repository<T>(_dbContext);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }

        public async Task RollBackTransactionAsync()
        {
            await _dbTransaction.RollbackAsync();
            await _dbTransaction.DisposeAsync();
            _dbTransaction = null!;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

    }

}