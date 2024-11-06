using Microsoft.EntityFrameworkCore.Storage;
using TeckShop.Core.Database;
using TeckShop.Core.Exceptions;

namespace TeckShop.Persistence.Database.EFCore
{
    /// <summary>
    /// The unit of work.
    /// </summary>
    /// <typeparam name="TContext"/>
    public class UnitOfWork<TContext> : IUnitOfWork
        where TContext : BaseDbContext
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly TContext _context;

        /// <summary>
        /// The transaction.
        /// </summary>
        private IDbContextTransaction? _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Begins the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="InvalidTransactionException">.</exception>
        /// <returns><![CDATA[Task<IDbContextTransaction>]]></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                throw new InvalidTransactionException("A transaction has already been started.");
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return _transaction;
        }

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="InvalidTransactionException">.</exception>
        /// <returns>A Task.</returns>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidTransactionException("A transaction has not been started.");
            await _transaction.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// Rollbacks the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="InvalidTransactionException">.</exception>
        /// <returns>A Task.</returns>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidTransactionException("A transaction has not been started.");
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// Save the changes asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
