namespace TeckShop.Core.Pagination
{
    /// <summary>
    /// The paged list.
    /// </summary>
    /// <typeparam name="T"/>
    public class PagedList<T>
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        public IList<T> Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="totalItems">The total items.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Page = pageNumber;
            Size = pageSize;
            TotalItems = totalItems;
            if (totalItems > 0)
            {
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            }

            Data = items as IList<T> ?? new List<T>(items);
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets the total items.
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Gets a value indicating whether first page.
        /// </summary>
        public bool IsFirstPage => Page == 1;

        /// <summary>
        /// Gets a value indicating whether last page.
        /// </summary>
        public bool IsLastPage => Page == TotalPages && TotalPages > 0;
    }
}
