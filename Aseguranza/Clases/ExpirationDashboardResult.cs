using System.Collections.Generic;

namespace Aseguranza.Clases
{
    public sealed class ExpirationDashboardResult
    {
        public ExpirationDashboardResult(
            IReadOnlyList<ExpirationEventRow> items,
            int totalCount,
            int pageNumber,
            int pageSize,
            ExpirationDashboardSummary summary)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Summary = summary;
        }

        public IReadOnlyList<ExpirationEventRow> Items { get; }
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public ExpirationDashboardSummary Summary { get; }
        public int TotalPages =>
            TotalCount == 0 ? 0 : (TotalCount - 1) / PageSize + 1;
    }
}
