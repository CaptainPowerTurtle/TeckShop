using Microsoft.EntityFrameworkCore.Storage;

namespace TeckShop.Core.Database
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begin a unit of work transaction.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commit a unit of work transaction.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rollback unit of work transaction.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Save unit of work changes.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
