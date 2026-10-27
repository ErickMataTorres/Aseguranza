using System.Data;

namespace Aseguranza.Clases
{
    public sealed class CertificationPageResult
    {
        public CertificationPageResult(
            DataTable items,
            int totalCount,
            int pageNumber,
            int pageSize,
            CertificationSummary summary)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Summary = summary;
        }

        public DataTable Items { get; }
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public CertificationSummary Summary { get; }
        public int TotalPages =>
            TotalCount == 0 ? 0 : (TotalCount - 1) / PageSize + 1;
    }
}
