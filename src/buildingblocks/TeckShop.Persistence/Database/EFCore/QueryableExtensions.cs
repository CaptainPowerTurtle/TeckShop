using Mapster;
using Microsoft.EntityFrameworkCore;
using TeckShop.Core.Pagination;

namespace TeckShop.Persistence.Database.EFCore
{
    /// <summary>
    /// The queryable extensions.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Apply the paging asynchronously.
        /// </summary>
        /// <typeparam name="T"/>
        /// <param name="collection">The collection.</param>
        /// <param name="page">The page.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<T>>]]></returns>
        public static async Task<PagedList<T>> ApplyPagingAsync<T>(this IQueryable<T> collection, int page = 1, int resultsPerPage = 10, CancellationToken cancellationToken = default)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }

            int skipSize = (page - 1) * resultsPerPage;
            bool isEmpty = !await collection.AnyAsync(cancellationToken: cancellationToken);
            if (isEmpty)
            {
                return new([], 0, 0, 0);
            }

            int totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
            List<T> data = [.. collection.Skip(skipSize).Take(resultsPerPage)];
            return new PagedList<T>(data, totalItems, page, resultsPerPage);
        }

        /// <summary>
        /// Apply the paging asynchronously.
        /// </summary>
        /// <typeparam name="T"/>
        /// <typeparam name="TDto"/>
        /// <param name="collection">The collection.</param>
        /// <param name="page">The page.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<PagedList<TDto>>]]></returns>
        public static async Task<PagedList<TDto>> ApplyPagingAsync<T, TDto>(this IQueryable<T> collection, int page = 1, int resultsPerPage = 10, CancellationToken cancellationToken = default)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }

            int skipSize = (page - 1) * resultsPerPage;
            bool isEmpty = !await collection.AnyAsync(cancellationToken: cancellationToken);
            if (isEmpty)
            {
                return new([], 0, 0, 0);
            }

            int totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
            IQueryable<TDto> data = collection.Skip(skipSize).Take(resultsPerPage).ProjectToType<TDto>();
            return new PagedList<TDto>(data, totalItems, page, resultsPerPage);
        }
    }
}
