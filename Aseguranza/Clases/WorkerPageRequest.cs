namespace Aseguranza.Clases
{
    public sealed class WorkerPageRequest
    {
        public const int DefaultPageSize = 100;

        public string Search { get; init; } = string.Empty;

        public bool? Active { get; init; } = true;

        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = DefaultPageSize;

        public int ValidPageNumber => Math.Max(1, PageNumber);

        public int ValidPageSize =>
            PageSize <= 0 ? DefaultPageSize : Math.Min(PageSize, DefaultPageSize);
    }
}
