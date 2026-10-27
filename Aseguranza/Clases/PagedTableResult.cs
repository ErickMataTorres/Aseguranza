using System.Data;

namespace Aseguranza.Clases
{
    public sealed class PagedTableResult
    {
        public PagedTableResult(
            DataTable items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public DataTable Items { get; }

        public int TotalCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalPages =>
            TotalCount == 0 ? 0 : (TotalCount - 1) / PageSize + 1;
    }
}
