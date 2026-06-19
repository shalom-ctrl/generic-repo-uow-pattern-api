using generic_repo_uow_pattern.Repository;

namespace generic_repo_uow_pattern.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IProductRepository ProductRepository { get; }
        TRepository GetRepository<TRepository, TEntity>() where TRepository : class,
            IRepository<TEntity> where TEntity : class;
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();
    }
}
