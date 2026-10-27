using System;

namespace Aseguranza.Clases
{
    public sealed class CertificationPageRequest
    {
        public const int DefaultPageSize = 100;

        public string Search { get; init; } = string.Empty;
        public bool? Active { get; init; } = true;
        public string CertificationStatus { get; init; } = "Todas";
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = DefaultPageSize;
        public DateTime AsOfDate { get; init; } = DateTime.Today;

        public int ValidPageNumber => Math.Max(1, PageNumber);
        public int ValidPageSize =>
            PageSize <= 0 ? DefaultPageSize : Math.Min(PageSize, DefaultPageSize);
    }
}
